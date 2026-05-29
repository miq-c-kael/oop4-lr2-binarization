using System.Diagnostics;

namespace Binarization;

public partial class MainForm : Form
{
    private Bitmap? _source;

    public MainForm()
    {
        InitializeComponent();
        UpdateButtons();
    }

    private void btnOpen_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Все файлы|*.*",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            var loaded = new Bitmap(dlg.FileName);
            _source?.Dispose();
            _source = loaded;
            pictureSource.Image?.Dispose();
            pictureSource.Image = (Bitmap)_source.Clone();
            pictureResult.Image?.Dispose();
            pictureResult.Image = null;
            labelStats.Text = $"Загружено: {_source.Width}×{_source.Height}";
            UpdateButtons();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Не удалось открыть изображение: " + ex.Message,
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnGenerate_Click(object? sender, EventArgs e)
    {
        // Тестовое полотно 3000×2000 с градиентом + шумом — заведомо большое,
        // чтобы был виден эффект параллелизации.
        const int w = 3000, h = 2000;
        var bmp = new Bitmap(w, h);
        var rng = new Random(0);
        using (var g = Graphics.FromImage(bmp))
        using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, w, h), Color.Black, Color.White, 0f))
        {
            g.FillRectangle(brush, 0, 0, w, h);
        }
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                if ((x + y) % 13 == 0)
                    bmp.SetPixel(x, y, Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256)));
            }

        _source?.Dispose();
        _source = bmp;
        pictureSource.Image?.Dispose();
        pictureSource.Image = (Bitmap)_source.Clone();
        pictureResult.Image?.Dispose();
        pictureResult.Image = null;
        labelStats.Text = $"Сгенерировано: {w}×{h}";
        UpdateButtons();
    }

    private void btnProcess_Click(object? sender, EventArgs e)
    {
        if (_source == null) return;
        var threshold = (byte)numThreshold.Value;

        btnProcess.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try
        {
            var swSeq = Stopwatch.StartNew();
            using (var seqResult = Binarizer.BinarizeSequential(_source, threshold))
            {
                swSeq.Stop();
                // показываем именно параллельный результат — для последовательного только меряем время
            }

            var swPar = Stopwatch.StartNew();
            var parResult = Binarizer.BinarizeParallel(_source, threshold);
            swPar.Stop();

            pictureResult.Image?.Dispose();
            pictureResult.Image = parResult;

            var speedup = swPar.ElapsedMilliseconds > 0
                ? (double)swSeq.ElapsedMilliseconds / swPar.ElapsedMilliseconds
                : double.PositiveInfinity;

            labelStats.Text =
                $"Размер: {_source.Width}×{_source.Height}, порог: {threshold}\n" +
                $"Последовательно: {swSeq.ElapsedMilliseconds} мс    " +
                $"Параллельно ({Environment.ProcessorCount} потоков): {swPar.ElapsedMilliseconds} мс\n" +
                $"Ускорение: ×{speedup:F2}";
        }
        finally
        {
            Cursor = Cursors.Default;
            UpdateButtons();
        }
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (pictureResult.Image == null) return;

        using var dlg = new SaveFileDialog
        {
            Filter = "PNG|*.png|JPEG|*.jpg|Bitmap|*.bmp",
            DefaultExt = "png",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        pictureResult.Image.Save(dlg.FileName);
    }

    private void UpdateButtons()
    {
        btnProcess.Enabled = _source != null;
        btnSave.Enabled = pictureResult.Image != null;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _source?.Dispose();
        pictureSource.Image?.Dispose();
        pictureResult.Image?.Dispose();
        base.OnFormClosed(e);
    }
}
