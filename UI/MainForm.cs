using System.Drawing.Imaging;
using PixelLab14.Core;

namespace PixelLab14.UI;

/// <summary>
/// Логика главной формы. Разметка и свойства элементов находятся в MainForm.Designer.cs.
/// Все обращения к элементам интерфейса выполняются в UI-потоке.
/// </summary>
public partial class MainForm : Form
{
    // Исходное изображение и последний успешно полученный результат.
    private PixelImage? _source;
    private PixelImage? _result;

    // Состояние текущей операции: источник отмены, занятость и номер запуска.
    private CancellationTokenSource? _operationCts;
    private bool _busy;
    private bool _closeRequested;
    private int _operationVersion;
    private string? _sourceFileName;

    public MainForm()
    {
        // Стандартный метод WinForms: создаёт элементы из файла Designer.cs.
        InitializeComponent();
    }

    // Обработчики событий, назначенные в визуальном конструкторе.
    // async void допустим для событий WinForms; сами операции возвращают Task.
    private async void OpenButton_Click(object? sender, EventArgs e) => await OpenAsync();
    private async void RunButton_Click(object? sender, EventArgs e) => await ProcessAsync();
    private async void SaveButton_Click(object? sender, EventArgs e) => await SaveAsync();
    private async void BenchmarkButton_Click(object? sender, EventArgs e) => await BenchmarkAsync();
    private void CancelButton_Click(object? sender, EventArgs e) => CancelOperation();

    // Не разрешаем запускать две операции одновременно. Доступность кнопок
    // также зависит от наличия исходного изображения и готового результата.
    private void SetBusy(bool busy)
    {
        _busy = busy;
        _openButton.Enabled = !busy;
        _runButton.Enabled = !busy && _source is not null;
        _saveButton.Enabled = !busy && _result is not null;
        _benchmarkButton.Enabled = !busy && _source is not null;
        _blockSize.Enabled = !busy;
        _workerCount.Enabled = !busy;
        _cancelButton.Enabled = busy;
    }

    // Создаём собственный токен для каждого запуска и сбрасываем индикатор.
    private CancellationToken BeginOperation(string message)
    {
        _operationVersion++;
        _operationCts = new CancellationTokenSource();
        SetBusy(true);
        _progress.Value = 0;
        _status.Text = message;
        return _operationCts.Token;
    }

    // Возвращаем интерфейс в рабочее состояние после успеха, ошибки или отмены.
    private void EndOperation()
    {
        // Инвалидируем запоздавшие сообщения Progress<T> из предыдущего запуска.
        _operationVersion++;
        _operationCts?.Dispose();
        _operationCts = null;
        SetBusy(false);
        if (_closeRequested) BeginInvoke(new Action(Close));
    }

    // Отмена кооперативная: сообщаем задачам о запросе, но не уничтожаем потоки.
    private void CancelOperation()
    {
        if (_operationCts is not { IsCancellationRequested: false } cts) return;
        cts.Cancel();
        _status.Text = "Отмена: завершаются рабочие задачи...";
        Append("Запрошена отмена операции.");
    }

    // Журнал хранит историю операций с временем каждого сообщения.
    private void Append(string text) => _log.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");

    // Заменяем изображение и освобождаем старый Bitmap, чтобы не расходовать память.
    private static void SetPicture(PictureBox box, Bitmap bitmap)
    {
        var previous = box.Image;
        box.Image = bitmap;
        previous?.Dispose();
        box.Invalidate();
    }

    // Чтение файла асинхронное; декодирование и создание Bitmap выполняются в пуле.
    private async Task OpenAsync()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Открыть изображение",
            Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Все файлы|*.*"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        var token = BeginOperation("Загрузка изображения...");
        try
        {
            // await возвращает управление циклу сообщений, пока чтение не закончено.
            var bytes = await File.ReadAllBytesAsync(dialog.FileName, token);
            // Декодирование CPU-зависимое, поэтому переносим его с UI-потока.
            var loaded = await Task.Run(() => PixelImage.Decode(bytes), token);
            token.ThrowIfCancellationRequested();
            using var disposablePreview = await Task.Run(loaded.ToBitmap, token);
            token.ThrowIfCancellationRequested();

            _source = loaded;
            _sourceFileName = Path.GetFileName(dialog.FileName);
            SetPicture(_originalPicture, (Bitmap)disposablePreview.Clone());
            _originalInfo.Text = $"{_sourceFileName} · {loaded.Width} × {loaded.Height} пикселей";
            // СТАРЫЙ РЕЗУЛЬТАТ НЕ ОЧИЩАЕТСЯ, даже если выбрана другая картинка.
            if (_result is not null)
                _resultInfo.Text = "Предыдущий результат (до новой обработки)";
            _status.Text = "Изображение загружено. Можно начать обработку.";
            Append($"Открыто: {_sourceFileName} ({loaded.Width}×{loaded.Height}).");
        }
        catch (OperationCanceledException) { ShowCancelled(); }
        catch (Exception ex) { ShowError("Ошибка загрузки", ex); }
        finally { EndOperation(); }
    }

    // Запуск пикселизации. До полного успеха прежний результат остаётся на экране.
    private async Task ProcessAsync()
    {
        if (_source is null) return;
        var token = BeginOperation("Обработка изображения...");
        var version = _operationVersion;
        var block = (int)_blockSize.Value;
        var workers = (int)_workerCount.Value;
        try
        {
            // UI показывает прежний результат до полной готовности нового изображения.
            // Progress<int> доставляет сообщения в контекст UI-потока.
            // Номер запуска защищает от запоздавших сообщений.
            var progress = new Progress<int>(value =>
            {
                if (!_busy || token.IsCancellationRequested || _operationVersion != version) return;
                // Сообщения от разных задач могут прийти не по порядку.
                _progress.Value = Math.Max(_progress.Value, Math.Clamp(value, 0, 100));
                _status.Text = $"Пикселизация: {_progress.Value}%";
            });
            // Ожидаем завершения рабочих задач без блокировки интерфейса.
            var processed = await PixelationProcessor.ProcessAsync(_source, block, workers, progress, token);
            if (processed is null) { ShowCancelled(); return; }

            var preview = await Task.Run(processed.Image.ToBitmap, token);
            if (token.IsCancellationRequested)
            {
                preview.Dispose();
                ShowCancelled();
                return;
            }

            // Единая точка публикации: результат и картинка обновляются только после успеха.
            _result = processed.Image;
            SetPicture(_resultPicture, preview);
            _resultInfo.Text = $"N = {block} · потоков: {workers} · {processed.Image.Width} × {processed.Image.Height}";
            _progress.Value = 100;
            _status.Text = $"Готово за {processed.Elapsed.TotalMilliseconds:F1} мс.";
            Append($"Пикселизация N={block}, потоков={workers}: {processed.Elapsed.TotalMilliseconds:F1} мс.");
        }
        catch (OperationCanceledException) { ShowCancelled(); }
        catch (Exception ex) { ShowError("Ошибка обработки", ex); }
        finally { EndOperation(); }
    }

    // Кодируем изображение в фоне и записываем файл асинхронно.
    private async Task SaveAsync()
    {
        if (_result is null) return;
        using var dialog = new SaveFileDialog
        {
            Title = "Сохранить результат",
            Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg",
            FileName = "pixelated.png",
            DefaultExt = "png",
            AddExtension = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        var token = BeginOperation("Сохранение результата...");
        var target = Path.GetFullPath(dialog.FileName);
        var temp = Path.Combine(Path.GetDirectoryName(target)!, $".{Guid.NewGuid():N}.tmp");
        try
        {
            var image = _result;
            var format = dialog.FilterIndex == 2 ? ImageFormat.Jpeg : ImageFormat.Png;
            var data = await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                using var bitmap = image.ToBitmap();
                using var stream = new MemoryStream();
                bitmap.Save(stream, format);
                return stream.ToArray();
            }, token);
            token.ThrowIfCancellationRequested();

            // Файл сначала пишется во временное имя: при отмене результат не портится.
            await File.WriteAllBytesAsync(temp, data, token);
            token.ThrowIfCancellationRequested();
            File.Move(temp, target, overwrite: true);
            _status.Text = "Результат сохранён.";
            Append($"Сохранено: {target}");
        }
        catch (OperationCanceledException) { ShowCancelled(); }
        catch (Exception ex) { ShowError("Ошибка сохранения", ex); }
        finally
        {
            try { if (File.Exists(temp)) File.Delete(temp); }
            catch (IOException ex) { Append($"Ошибка удаления временного файла: {ex.Message}"); }
            EndOperation();
        }
    }

    // Сравниваем 1, 2, 4 и 8 рабочих задач на одинаковом исходнике и размере блока.
    private async Task BenchmarkAsync()
    {
        if (_source is null) return;
        var token = BeginOperation("Тестирование производительности...");
        var block = (int)_blockSize.Value;
        try
        {
            Append($"Тест {_source.Width}×{_source.Height}, N={block} (время только обработки):");
            // Каждый тест использует один и тот же исходный буфер, не меняя результат справа.
            var counts = new[] { 1, 2, 4, 8 };
            for (var i = 0; i < counts.Length; i++)
            {
                if (token.IsCancellationRequested) { ShowCancelled(); return; }
                var count = counts[i];
                _status.Text = $"Тест: {count} поток(а/ов)...";
                var result = await PixelationProcessor.ProcessAsync(_source, block, count, null, token);
                if (result is null) { ShowCancelled(); return; }
                Append($"  {count} поток(а/ов): {result.Elapsed.TotalMilliseconds:F1} мс");
                _progress.Value = (i + 1) * 25;
            }
            _status.Text = "Тест завершён. Результаты доступны в журнале.";
        }
        catch (OperationCanceledException) { ShowCancelled(); }
        catch (Exception ex) { ShowError("Ошибка тестирования", ex); }
        finally { EndOperation(); }
    }

    // Отмена не считается ошибкой. Результат предыдущей успешной операции сохраняется.
    private void ShowCancelled()
    {
        _status.Text = "Операция отменена. Предыдущий результат сохранён.";
        _progress.Value = 0;
        Append("Отменено. Рабочие задачи завершены.");
    }

    // Ошибку отображаем в журнале и диалоге, не завершая приложение.
    private void ShowError(string caption, Exception ex)
    {
        _status.Text = caption;
        Append($"ОШИБКА: {ex.Message}");
        MessageBox.Show(this, ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // При закрытии во время работы сначала отменяем операцию и ждём её завершения.
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_busy)
        {
            // Пока рабочие задачи завершаются, не уничтожаем элементы интерфейса.
            e.Cancel = true;
            _closeRequested = true;
            CancelOperation();
            return;
        }
        _originalPicture.Image?.Dispose();
        _resultPicture.Image?.Dispose();
        base.OnFormClosing(e);
    }

    private void _originalPicture_Click(object sender, EventArgs e)
    {

    }
}
