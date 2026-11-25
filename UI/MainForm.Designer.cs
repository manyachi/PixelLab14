using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelLab14.UI;

partial class MainForm
{
    private IContainer? components = null;

    /// <summary>Освобождение компонентов, принадлежащих форме.</summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>Разметка окна. Редактируется через визуальный конструктор WinForms.</summary>
    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        headingLayout = new TableLayoutPanel();
        titleLabel = new Label();
        subtitleLabel = new Label();
        toolbar = new FlowLayoutPanel();
        _openButton = new Button();
        blockLabel = new Label();
        _blockSize = new NumericUpDown();
        workersLabel = new Label();
        _workerCount = new NumericUpDown();
        _runButton = new Button();
        _cancelButton = new Button();
        _saveButton = new Button();
        _benchmarkButton = new Button();
        _verticalSplit = new SplitContainer();
        picturesLayout = new TableLayoutPanel();
        originalLayout = new TableLayoutPanel();
        originalTitle = new Label();
        _originalPicture = new PictureBox();
        _originalInfo = new Label();
        resultLayout = new TableLayoutPanel();
        resultTitle = new Label();
        _resultPicture = new PictureBox();
        _resultInfo = new Label();
        journalLayout = new TableLayoutPanel();
        journalTitle = new Label();
        _log = new TextBox();
        footerLayout = new TableLayoutPanel();
        _status = new Label();
        _progress = new ProgressBar();
        rootLayout.SuspendLayout();
        headingLayout.SuspendLayout();
        toolbar.SuspendLayout();
        ((ISupportInitialize)_blockSize).BeginInit();
        ((ISupportInitialize)_workerCount).BeginInit();
        ((ISupportInitialize)_verticalSplit).BeginInit();
        _verticalSplit.Panel1.SuspendLayout();
        _verticalSplit.Panel2.SuspendLayout();
        _verticalSplit.SuspendLayout();
        picturesLayout.SuspendLayout();
        originalLayout.SuspendLayout();
        ((ISupportInitialize)_originalPicture).BeginInit();
        resultLayout.SuspendLayout();
        ((ISupportInitialize)_resultPicture).BeginInit();
        journalLayout.SuspendLayout();
        footerLayout.SuspendLayout();
        SuspendLayout();
        // 
        // rootLayout
        // 
        rootLayout.BackColor = Color.FromArgb(245, 247, 250);
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(headingLayout, 0, 0);
        rootLayout.Controls.Add(toolbar, 0, 1);
        rootLayout.Controls.Add(_verticalSplit, 0, 2);
        rootLayout.Controls.Add(footerLayout, 0, 3);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(18, 14, 18, 14);
        rootLayout.RowCount = 4;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        rootLayout.Size = new Size(1264, 801);
        rootLayout.TabIndex = 0;
        // 
        // headingLayout
        // 
        headingLayout.ColumnCount = 1;
        headingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        headingLayout.Controls.Add(titleLabel, 0, 0);
        headingLayout.Controls.Add(subtitleLabel, 0, 1);
        headingLayout.Dock = DockStyle.Fill;
        headingLayout.Location = new Point(21, 17);
        headingLayout.Name = "headingLayout";
        headingLayout.RowCount = 2;
        headingLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        headingLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        headingLayout.Size = new Size(1222, 56);
        headingLayout.TabIndex = 1;
        // 
        // titleLabel
        // 
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        titleLabel.ForeColor = Color.FromArgb(29, 42, 59);
        titleLabel.Location = new Point(3, 0);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(1216, 35);
        titleLabel.TabIndex = 2;
        titleLabel.Text = "Обработка изображений";
        titleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // subtitleLabel
        // 
        subtitleLabel.Dock = DockStyle.Fill;
        subtitleLabel.ForeColor = Color.FromArgb(103, 115, 132);
        subtitleLabel.Location = new Point(3, 35);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Size = new Size(1216, 21);
        subtitleLabel.TabIndex = 3;
        subtitleLabel.Text = "Пикселизация блоками N×N · многопоточность и асинхронный ввод-вывод";
        subtitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // toolbar
        // 
        toolbar.AutoScroll = true;
        toolbar.BackColor = Color.White;
        toolbar.Controls.Add(_openButton);
        toolbar.Controls.Add(blockLabel);
        toolbar.Controls.Add(_blockSize);
        toolbar.Controls.Add(workersLabel);
        toolbar.Controls.Add(_workerCount);
        toolbar.Controls.Add(_runButton);
        toolbar.Controls.Add(_cancelButton);
        toolbar.Controls.Add(_saveButton);
        toolbar.Controls.Add(_benchmarkButton);
        toolbar.Dock = DockStyle.Fill;
        toolbar.Location = new Point(21, 79);
        toolbar.Name = "toolbar";
        toolbar.Padding = new Padding(10, 14, 10, 8);
        toolbar.Size = new Size(1222, 68);
        toolbar.TabIndex = 4;
        toolbar.WrapContents = false;
        // 
        // _openButton
        // 
        _openButton.BackColor = Color.FromArgb(237, 242, 249);
        _openButton.FlatStyle = FlatStyle.Flat;
        _openButton.ForeColor = Color.FromArgb(40, 57, 76);
        _openButton.Location = new Point(14, 16);
        _openButton.Margin = new Padding(4, 2, 14, 2);
        _openButton.Name = "_openButton";
        _openButton.Padding = new Padding(7, 0, 7, 0);
        _openButton.Size = new Size(184, 35);
        _openButton.TabIndex = 5;
        _openButton.Text = "Открыть изображение";
        _openButton.UseVisualStyleBackColor = false;
        _openButton.Click += OpenButton_Click;
        // 
        // blockLabel
        // 
        blockLabel.AutoSize = true;
        blockLabel.ForeColor = Color.FromArgb(58, 71, 88);
        blockLabel.Location = new Point(215, 24);
        blockLabel.Margin = new Padding(3, 10, 6, 0);
        blockLabel.Name = "blockLabel";
        blockLabel.Size = new Size(65, 23);
        blockLabel.TabIndex = 10;
        blockLabel.Text = "Блок N";
        // 
        // _blockSize
        // 
        _blockSize.Location = new Point(289, 19);
        _blockSize.Margin = new Padding(3, 5, 10, 3);
        _blockSize.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
        _blockSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _blockSize.Name = "_blockSize";
        _blockSize.Size = new Size(68, 30);
        _blockSize.TabIndex = 12;
        _blockSize.Value = new decimal(new int[] { 8, 0, 0, 0 });
        // 
        // workersLabel
        // 
        workersLabel.AutoSize = true;
        workersLabel.ForeColor = Color.FromArgb(58, 71, 88);
        workersLabel.Location = new Point(370, 24);
        workersLabel.Margin = new Padding(3, 10, 6, 0);
        workersLabel.Name = "workersLabel";
        workersLabel.Size = new Size(67, 23);
        workersLabel.TabIndex = 11;
        workersLabel.Text = "Потоки";
        // 
        // _workerCount
        // 
        _workerCount.Location = new Point(446, 19);
        _workerCount.Margin = new Padding(3, 5, 10, 3);
        _workerCount.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
        _workerCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _workerCount.Name = "_workerCount";
        _workerCount.Size = new Size(68, 30);
        _workerCount.TabIndex = 13;
        _workerCount.Value = new decimal(new int[] { 4, 0, 0, 0 });
        // 
        // _runButton
        // 
        _runButton.BackColor = Color.FromArgb(37, 99, 235);
        _runButton.Enabled = false;
        _runButton.FlatStyle = FlatStyle.Flat;
        _runButton.ForeColor = Color.White;
        _runButton.Location = new Point(538, 16);
        _runButton.Margin = new Padding(14, 2, 4, 2);
        _runButton.Name = "_runButton";
        _runButton.Padding = new Padding(7, 0, 7, 0);
        _runButton.Size = new Size(126, 35);
        _runButton.TabIndex = 6;
        _runButton.Text = "Обработать";
        _runButton.UseVisualStyleBackColor = false;
        _runButton.Click += RunButton_Click;
        // 
        // _cancelButton
        // 
        _cancelButton.BackColor = Color.FromArgb(237, 242, 249);
        _cancelButton.Enabled = false;
        _cancelButton.FlatStyle = FlatStyle.Flat;
        _cancelButton.ForeColor = Color.FromArgb(40, 57, 76);
        _cancelButton.Location = new Point(672, 16);
        _cancelButton.Margin = new Padding(4, 2, 4, 2);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Padding = new Padding(7, 0, 7, 0);
        _cancelButton.Size = new Size(90, 35);
        _cancelButton.TabIndex = 7;
        _cancelButton.Text = "Отмена";
        _cancelButton.UseVisualStyleBackColor = false;
        _cancelButton.Click += CancelButton_Click;
        // 
        // _saveButton
        // 
        _saveButton.BackColor = Color.FromArgb(237, 242, 249);
        _saveButton.Enabled = false;
        _saveButton.FlatStyle = FlatStyle.Flat;
        _saveButton.ForeColor = Color.FromArgb(40, 57, 76);
        _saveButton.Location = new Point(770, 16);
        _saveButton.Margin = new Padding(4, 2, 14, 2);
        _saveButton.Name = "_saveButton";
        _saveButton.Padding = new Padding(7, 0, 7, 0);
        _saveButton.Size = new Size(184, 35);
        _saveButton.TabIndex = 8;
        _saveButton.Text = "Сохранить результат";
        _saveButton.UseVisualStyleBackColor = false;
        _saveButton.Click += SaveButton_Click;
        // 
        // _benchmarkButton
        // 
        _benchmarkButton.BackColor = Color.FromArgb(237, 242, 249);
        _benchmarkButton.Enabled = false;
        _benchmarkButton.FlatStyle = FlatStyle.Flat;
        _benchmarkButton.ForeColor = Color.FromArgb(40, 57, 76);
        _benchmarkButton.Location = new Point(972, 16);
        _benchmarkButton.Margin = new Padding(4, 2, 4, 2);
        _benchmarkButton.Name = "_benchmarkButton";
        _benchmarkButton.Padding = new Padding(7, 0, 7, 0);
        _benchmarkButton.Size = new Size(213, 35);
        _benchmarkButton.TabIndex = 9;
        _benchmarkButton.Text = "Тест 1 / 2 / 4 / 8 потоков";
        _benchmarkButton.UseVisualStyleBackColor = false;
        _benchmarkButton.Click += BenchmarkButton_Click;
        // 
        // _verticalSplit
        // 
        _verticalSplit.BackColor = Color.FromArgb(245, 247, 250);
        _verticalSplit.Dock = DockStyle.Fill;
        _verticalSplit.FixedPanel = FixedPanel.Panel2;
        _verticalSplit.Location = new Point(21, 153);
        _verticalSplit.Name = "_verticalSplit";
        _verticalSplit.Orientation = Orientation.Horizontal;
        // 
        // _verticalSplit.Panel1
        // 
        _verticalSplit.Panel1.Controls.Add(picturesLayout);
        _verticalSplit.Panel1MinSize = 180;
        // 
        // _verticalSplit.Panel2
        // 
        _verticalSplit.Panel2.Controls.Add(journalLayout);
        _verticalSplit.Panel2MinSize = 110;
        _verticalSplit.Size = new Size(1222, 593);
        _verticalSplit.SplitterDistance = 385;
        _verticalSplit.SplitterWidth = 7;
        _verticalSplit.TabIndex = 14;
        // 
        // picturesLayout
        // 
        picturesLayout.ColumnCount = 2;
        picturesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        picturesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        picturesLayout.Controls.Add(originalLayout, 0, 0);
        picturesLayout.Controls.Add(resultLayout, 1, 0);
        picturesLayout.Dock = DockStyle.Fill;
        picturesLayout.Location = new Point(0, 0);
        picturesLayout.Name = "picturesLayout";
        picturesLayout.Padding = new Padding(0, 12, 0, 4);
        picturesLayout.RowCount = 1;
        picturesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        picturesLayout.Size = new Size(1222, 385);
        picturesLayout.TabIndex = 15;
        // 
        // originalLayout
        // 
        originalLayout.BackColor = Color.White;
        originalLayout.ColumnCount = 1;
        originalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        originalLayout.Controls.Add(originalTitle, 0, 0);
        originalLayout.Controls.Add(_originalPicture, 0, 1);
        originalLayout.Controls.Add(_originalInfo, 0, 2);
        originalLayout.Dock = DockStyle.Fill;
        originalLayout.Location = new Point(3, 12);
        originalLayout.Margin = new Padding(3, 0, 10, 0);
        originalLayout.Name = "originalLayout";
        originalLayout.Padding = new Padding(12, 8, 12, 8);
        originalLayout.RowCount = 3;
        originalLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        originalLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        originalLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        originalLayout.Size = new Size(598, 369);
        originalLayout.TabIndex = 16;
        // 
        // originalTitle
        // 
        originalTitle.Dock = DockStyle.Fill;
        originalTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        originalTitle.ForeColor = Color.FromArgb(65, 77, 94);
        originalTitle.Location = new Point(15, 8);
        originalTitle.Name = "originalTitle";
        originalTitle.Size = new Size(568, 32);
        originalTitle.TabIndex = 17;
        originalTitle.Text = "ОРИГИНАЛ";
        originalTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _originalPicture
        // 
        _originalPicture.BackColor = Color.FromArgb(245, 247, 250);
        _originalPicture.Dock = DockStyle.Fill;
        _originalPicture.Location = new Point(15, 43);
        _originalPicture.Name = "_originalPicture";
        _originalPicture.Size = new Size(568, 287);
        _originalPicture.SizeMode = PictureBoxSizeMode.Zoom;
        _originalPicture.TabIndex = 18;
        _originalPicture.TabStop = false;
        _originalPicture.Click += _originalPicture_Click;
        // 
        // _originalInfo
        // 
        _originalInfo.AutoEllipsis = true;
        _originalInfo.Dock = DockStyle.Fill;
        _originalInfo.ForeColor = Color.FromArgb(100, 112, 131);
        _originalInfo.Location = new Point(15, 333);
        _originalInfo.Name = "_originalInfo";
        _originalInfo.Size = new Size(568, 28);
        _originalInfo.TabIndex = 19;
        _originalInfo.Text = "Изображение не выбрано";
        _originalInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // resultLayout
        // 
        resultLayout.BackColor = Color.White;
        resultLayout.ColumnCount = 1;
        resultLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        resultLayout.Controls.Add(resultTitle, 0, 0);
        resultLayout.Controls.Add(_resultPicture, 0, 1);
        resultLayout.Controls.Add(_resultInfo, 0, 2);
        resultLayout.Dock = DockStyle.Fill;
        resultLayout.Location = new Point(614, 12);
        resultLayout.Margin = new Padding(3, 0, 10, 0);
        resultLayout.Name = "resultLayout";
        resultLayout.Padding = new Padding(12, 8, 12, 8);
        resultLayout.RowCount = 3;
        resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        resultLayout.Size = new Size(598, 369);
        resultLayout.TabIndex = 20;
        // 
        // resultTitle
        // 
        resultTitle.Dock = DockStyle.Fill;
        resultTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        resultTitle.ForeColor = Color.FromArgb(65, 77, 94);
        resultTitle.Location = new Point(15, 8);
        resultTitle.Name = "resultTitle";
        resultTitle.Size = new Size(568, 32);
        resultTitle.TabIndex = 21;
        resultTitle.Text = "РЕЗУЛЬТАТ";
        resultTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _resultPicture
        // 
        _resultPicture.BackColor = Color.FromArgb(245, 247, 250);
        _resultPicture.Dock = DockStyle.Fill;
        _resultPicture.Location = new Point(15, 43);
        _resultPicture.Name = "_resultPicture";
        _resultPicture.Size = new Size(568, 287);
        _resultPicture.SizeMode = PictureBoxSizeMode.Zoom;
        _resultPicture.TabIndex = 22;
        _resultPicture.TabStop = false;
        // 
        // _resultInfo
        // 
        _resultInfo.AutoEllipsis = true;
        _resultInfo.Dock = DockStyle.Fill;
        _resultInfo.ForeColor = Color.FromArgb(100, 112, 131);
        _resultInfo.Location = new Point(15, 333);
        _resultInfo.Name = "_resultInfo";
        _resultInfo.Size = new Size(568, 28);
        _resultInfo.TabIndex = 23;
        _resultInfo.Text = "Результата пока нет";
        _resultInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // journalLayout
        // 
        journalLayout.BackColor = Color.White;
        journalLayout.ColumnCount = 1;
        journalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        journalLayout.Controls.Add(journalTitle, 0, 0);
        journalLayout.Controls.Add(_log, 0, 1);
        journalLayout.Dock = DockStyle.Fill;
        journalLayout.Location = new Point(0, 0);
        journalLayout.Name = "journalLayout";
        journalLayout.Padding = new Padding(12, 8, 12, 10);
        journalLayout.RowCount = 2;
        journalLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        journalLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        journalLayout.Size = new Size(1222, 201);
        journalLayout.TabIndex = 24;
        // 
        // journalTitle
        // 
        journalTitle.Dock = DockStyle.Fill;
        journalTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        journalTitle.ForeColor = Color.FromArgb(65, 77, 94);
        journalTitle.Location = new Point(15, 8);
        journalTitle.Name = "journalTitle";
        journalTitle.Size = new Size(1192, 28);
        journalTitle.TabIndex = 25;
        journalTitle.Text = "ЖУРНАЛ И ИЗМЕРЕНИЯ";
        journalTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _log
        // 
        _log.BackColor = Color.White;
        _log.BorderStyle = BorderStyle.None;
        _log.Dock = DockStyle.Fill;
        _log.Font = new Font("Consolas", 9.5F);
        _log.Location = new Point(15, 39);
        _log.Multiline = true;
        _log.Name = "_log";
        _log.ReadOnly = true;
        _log.ScrollBars = ScrollBars.Both;
        _log.Size = new Size(1192, 149);
        _log.TabIndex = 26;
        _log.WordWrap = false;
        // 
        // footerLayout
        // 
        footerLayout.ColumnCount = 2;
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
        footerLayout.Controls.Add(_status, 0, 0);
        footerLayout.Controls.Add(_progress, 1, 0);
        footerLayout.Dock = DockStyle.Fill;
        footerLayout.Location = new Point(21, 752);
        footerLayout.Name = "footerLayout";
        footerLayout.Padding = new Padding(2, 6, 0, 0);
        footerLayout.RowCount = 1;
        footerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        footerLayout.Size = new Size(1222, 32);
        footerLayout.TabIndex = 27;
        // 
        // _status
        // 
        _status.Dock = DockStyle.Fill;
        _status.Location = new Point(5, 6);
        _status.Name = "_status";
        _status.Size = new Size(1014, 26);
        _status.TabIndex = 28;
        _status.Text = "Откройте изображение для начала работы.";
        _status.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _progress
        // 
        _progress.Dock = DockStyle.Fill;
        _progress.Location = new Point(1025, 9);
        _progress.Name = "_progress";
        _progress.Size = new Size(194, 20);
        _progress.TabIndex = 29;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1264, 801);
        Controls.Add(rootLayout);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(920, 620);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PixelLab — лабораторная №4 · вариант 14";
        rootLayout.ResumeLayout(false);
        headingLayout.ResumeLayout(false);
        toolbar.ResumeLayout(false);
        toolbar.PerformLayout();
        ((ISupportInitialize)_blockSize).EndInit();
        ((ISupportInitialize)_workerCount).EndInit();
        _verticalSplit.Panel1.ResumeLayout(false);
        _verticalSplit.Panel2.ResumeLayout(false);
        ((ISupportInitialize)_verticalSplit).EndInit();
        _verticalSplit.ResumeLayout(false);
        picturesLayout.ResumeLayout(false);
        originalLayout.ResumeLayout(false);
        ((ISupportInitialize)_originalPicture).EndInit();
        resultLayout.ResumeLayout(false);
        ((ISupportInitialize)_resultPicture).EndInit();
        journalLayout.ResumeLayout(false);
        journalLayout.PerformLayout();
        footerLayout.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel rootLayout = null!;
    private TableLayoutPanel headingLayout = null!;
    private Label titleLabel = null!;
    private Label subtitleLabel = null!;
    private FlowLayoutPanel toolbar = null!;
    private Button _openButton = null!;
    private Button _runButton = null!;
    private Button _cancelButton = null!;
    private Button _saveButton = null!;
    private Button _benchmarkButton = null!;
    private Label blockLabel = null!;
    private Label workersLabel = null!;
    private NumericUpDown _blockSize = null!;
    private NumericUpDown _workerCount = null!;
    private SplitContainer _verticalSplit = null!;
    private TableLayoutPanel picturesLayout = null!;
    private TableLayoutPanel originalLayout = null!;
    private Label originalTitle = null!;
    private PictureBox _originalPicture = null!;
    private Label _originalInfo = null!;
    private TableLayoutPanel resultLayout = null!;
    private Label resultTitle = null!;
    private PictureBox _resultPicture = null!;
    private Label _resultInfo = null!;
    private TableLayoutPanel journalLayout = null!;
    private Label journalTitle = null!;
    private TextBox _log = null!;
    private TableLayoutPanel footerLayout = null!;
    private Label _status = null!;
    private ProgressBar _progress = null!;
}
