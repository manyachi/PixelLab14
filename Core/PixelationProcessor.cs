using System.Diagnostics;

namespace PixelLab14.Core;

/// <summary>Результат: обработанное изображение и затраченное время по секундомеру.</summary>
internal sealed record ProcessingResult(PixelImage Image, TimeSpan Elapsed);

/// <summary>
/// Лабораторная работа №4, вариант 14: пикселизация N×N средним цветом.
/// Пул из фиксированного числа задач работает через общий потокобезопасный доступ
/// к очереди. Для координации применены lock, SemaphoreSlim и CountdownEvent.
/// </summary>
internal static class PixelationProcessor
{
    // Рабочая единица: прямоугольник, содержащий целые блоки N×N.
    private sealed record Tile(int X0, int Y0, int X1, int Y1);

    public static async Task<ProcessingResult?> ProcessAsync(
        PixelImage source,
        int blockSize,
        int workerCount,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        // Допустимые диапазоны совпадают с параметрами на форме.
        if (blockSize < 1 || blockSize > 256)
            throw new ArgumentOutOfRangeException(nameof(blockSize));
        if (workerCount < 1 || workerCount > 32)
            throw new ArgumentOutOfRangeException(nameof(workerCount));

        if (cancellationToken.IsCancellationRequested) return null;
        // Исходные пиксели только читаются; результат записывается в отдельный массив.
        var output = new byte[source.Pixels.Length];

        // Размер фрагмента кратен N: разные потоки никогда не пишут в один блок.
        // 256 — ориентир размера фрагмента, а не предел разрешения изображения.
        // Целочисленное деление гарантирует кратность стороны фрагмента размеру N.
        var blocksPerTile = Math.Max(1, 256 / blockSize);
        var tileSide = checked(blocksPerTile * blockSize);
        var queue = new Queue<Tile?>();
        for (var y = 0; y < source.Height; y += tileSide)
        for (var x = 0; x < source.Width; x += tileSide)
            queue.Enqueue(new Tile(x, y,
                Math.Min(x + tileSide, source.Width),
                Math.Min(y + tileSide, source.Height)));

        // Не создаём рабочих задач больше, чем фактических фрагментов.
        var jobCount = queue.Count;
        var workers = Math.Min(workerCount, jobCount);

        // null в очереди — команда рабочему потоку завершиться.
        for (var i = 0; i < workers; i++)
            queue.Enqueue(null);

        // У критической секции один общий объект блокировки.
        var gate = new object();
        var completed = 0;
        using var jobsAvailable = new SemaphoreSlim(queue.Count, queue.Count);
        using var allJobsDone = new CountdownEvent(jobCount);
        // Замер начинается после построения очереди; загрузка и отображение не входят.
        var clock = Stopwatch.StartNew();

        // Каждый Worker выполняется в штатном пуле ThreadPool.
        // Количество рабочих задач фиксировано и задаётся пользователем.
        var tasks = new Task[workers];
        for (var i = 0; i < workers; i++)
        {
            tasks[i] = Task.Run(() => Worker(), CancellationToken.None);
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
        // Отмена является штатным результатом, а не аварией в рабочих потоках.
        // Сначала Task.WhenAll дожидается ВСЕХ рабочих задач, только потом
        // возвращаем null (частично обработанный буфер не показываем).
        if (cancellationToken.IsCancellationRequested) return null;
        // Проверка подтверждает, что обработаны ВСЕ фрагменты.
        if (!allJobsDone.IsSet)
            throw new InvalidOperationException("Не все фрагменты были обработаны.");

        clock.Stop();
        progress?.Report(100);
        return new ProcessingResult(new PixelImage(source.Width, source.Height, output), clock.Elapsed);

        void Worker()
        {
            while (true)
            {
                // SemaphoreSlim — механизм синхронизации №1:
                // ожидаем доступное задание без постоянного опроса очереди.
                // В очереди заранее лежат все задания и команды завершения.
                // Поэтому Wait() здесь не может бесконечно ждать: для каждой
                // рабочей задачи есть минимум один элемент. Отмену проверяем
                // отдельно, без выбрасывания исключения в фоновом потоке.
                if (cancellationToken.IsCancellationRequested) return;
                jobsAvailable.Wait();
                if (cancellationToken.IsCancellationRequested) return;

                Tile? tile;
                lock (gate) // КРИТИЧЕСКАЯ СЕКЦИЯ: только один поток извлекает задание.
                {
                    tile = queue.Dequeue();
                }
                if (tile is null)
                    return;

                if (!ProcessTile(source, output, tile, blockSize, cancellationToken))
                    return; // Отменено: незаконченный фрагмент не учитываем.
                if (cancellationToken.IsCancellationRequested) return;

                int percent;
                lock (gate) // КРИТИЧЕСКАЯ СЕКЦИЯ: счётчик и событие завершения.
                {
                    completed++;
                    allJobsDone.Signal(); // CountdownEvent — механизм №2.
                    percent = completed * 100 / jobCount;
                }
                // UI обновляется через Progress<T> вне блокировки.
                progress?.Report(percent);
            }
        }
    }

    /// <summary>
    /// Каждый блок N×N заменяется одним усреднённым цветом (включая альфа-канал).
    /// На правом и нижнем краю блок может быть меньше N×N.
    /// </summary>
    private static bool ProcessTile(
        PixelImage source, byte[] output, Tile tile, int n, CancellationToken ct)
    {
        var input = source.Pixels;
        var width = source.Width;

        for (var by = tile.Y0; by < tile.Y1; by += n)
        {
            if (ct.IsCancellationRequested) return false; // Быстрая отмена без исключения.
            var yEnd = Math.Min(by + n, source.Height);
            for (var bx = tile.X0; bx < tile.X1; bx += n)
            {
                var xEnd = Math.Min(bx + n, source.Width);
                // Суммы храним в long: байт не вместит сумму многих пикселей.
                long b = 0, g = 0, r = 0, a = 0;
                var count = (xEnd - bx) * (yEnd - by);

                // 1. Находим сумму значений каналов для всего блока.
                for (var y = by; y < yEnd; y++)
                for (var x = bx; x < xEnd; x++)
                {
                    // Переход от двумерных координат к индексу в массиве BGRA.
                    var p = (y * width + x) * 4;
                    b += input[p];
                    g += input[p + 1];
                    r += input[p + 2];
                    a += input[p + 3];
                }

                // 2. Средний цвет — арифметическое среднее по каждому каналу.
                var bb = (byte)(b / count);
                var gg = (byte)(g / count);
                var rr = (byte)(r / count);
                var aa = (byte)(a / count);

                // 3. Все пиксели внутри блока получают одинаковый цвет.
                for (var y = by; y < yEnd; y++)
                for (var x = bx; x < xEnd; x++)
                {
                    var p = (y * width + x) * 4;
                    output[p] = bb;
                    output[p + 1] = gg;
                    output[p + 2] = rr;
                    output[p + 3] = aa;
                }
            }
        }
        return !ct.IsCancellationRequested;
    }
}
