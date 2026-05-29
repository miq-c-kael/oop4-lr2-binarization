#nullable enable
namespace Binarization;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private Button btnOpen = null!;
    private Button btnGenerate = null!;
    private Label labelThreshold = null!;
    private NumericUpDown numThreshold = null!;
    private Button btnProcess = null!;
    private Button btnSave = null!;
    private PictureBox pictureSource = null!;
    private PictureBox pictureResult = null!;
    private Label labelStats = null!;
    private Label labelSource = null!;
    private Label labelResult = null!;

    private void InitializeComponent()
    {
        btnOpen = new Button();
        btnGenerate = new Button();
        labelThreshold = new Label();
        numThreshold = new NumericUpDown();
        btnProcess = new Button();
        btnSave = new Button();
        pictureSource = new PictureBox();
        pictureResult = new PictureBox();
        labelStats = new Label();
        labelSource = new Label();
        labelResult = new Label();

        ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureSource).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureResult).BeginInit();
        SuspendLayout();

        btnOpen.Location = new Point(12, 12);
        btnOpen.Size = new Size(140, 30);
        btnOpen.Text = "Открыть...";
        btnOpen.Click += btnOpen_Click;

        btnGenerate.Location = new Point(158, 12);
        btnGenerate.Size = new Size(180, 30);
        btnGenerate.Text = "Сгенерировать тест";
        btnGenerate.Click += btnGenerate_Click;

        labelThreshold.Location = new Point(360, 12);
        labelThreshold.Size = new Size(60, 30);
        labelThreshold.TextAlign = ContentAlignment.MiddleRight;
        labelThreshold.Text = "Порог:";

        numThreshold.Location = new Point(425, 16);
        numThreshold.Size = new Size(60, 23);
        numThreshold.Minimum = 0;
        numThreshold.Maximum = 255;
        numThreshold.Value = 128;

        btnProcess.Location = new Point(495, 12);
        btnProcess.Size = new Size(170, 30);
        btnProcess.Text = "Бинаризовать";
        btnProcess.Click += btnProcess_Click;

        btnSave.Location = new Point(670, 12);
        btnSave.Size = new Size(110, 30);
        btnSave.Text = "Сохранить...";
        btnSave.Click += btnSave_Click;

        labelSource.Location = new Point(12, 50);
        labelSource.Size = new Size(380, 18);
        labelSource.Text = "Источник";
        labelSource.TextAlign = ContentAlignment.MiddleCenter;

        labelResult.Location = new Point(398, 50);
        labelResult.Size = new Size(380, 18);
        labelResult.Text = "Результат";
        labelResult.TextAlign = ContentAlignment.MiddleCenter;

        pictureSource.Location = new Point(12, 70);
        pictureSource.Size = new Size(380, 350);
        pictureSource.BorderStyle = BorderStyle.FixedSingle;
        pictureSource.SizeMode = PictureBoxSizeMode.Zoom;

        pictureResult.Location = new Point(398, 70);
        pictureResult.Size = new Size(380, 350);
        pictureResult.BorderStyle = BorderStyle.FixedSingle;
        pictureResult.SizeMode = PictureBoxSizeMode.Zoom;

        labelStats.Location = new Point(12, 430);
        labelStats.Size = new Size(766, 60);
        labelStats.Font = new Font("Consolas", 9F);
        labelStats.Text = "Откройте или сгенерируйте изображение";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(790, 500);
        Controls.Add(btnOpen);
        Controls.Add(btnGenerate);
        Controls.Add(labelThreshold);
        Controls.Add(numThreshold);
        Controls.Add(btnProcess);
        Controls.Add(btnSave);
        Controls.Add(labelSource);
        Controls.Add(labelResult);
        Controls.Add(pictureSource);
        Controls.Add(pictureResult);
        Controls.Add(labelStats);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ЛР 2 — Бинаризация по порогу (вариант 8)";

        ((System.ComponentModel.ISupportInitialize)numThreshold).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureSource).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureResult).EndInit();
        ResumeLayout(false);
    }
}
