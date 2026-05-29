using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Binarization;

/// <summary>
/// Бинаризация изображения по порогу:
/// яркость каждого пикселя сравнивается с порогом; больше — белый, иначе чёрный.
/// Яркость: Y = 0.299·R + 0.587·G + 0.114·B (Rec.601).
/// Работает напрямую с пиксельным буфером через LockBits 
/// быстрее GetPixel/SetPixel и пригодно для параллельной обработки.
/// </summary>
public static class Binarizer
{
    public static Bitmap BinarizeSequential(Bitmap source, byte threshold)
        => Binarize(source, threshold, parallel: false);

    public static Bitmap BinarizeParallel(Bitmap source, byte threshold, int? degreeOfParallelism = null)
        => Binarize(source, threshold, parallel: true, degreeOfParallelism);

    private static unsafe Bitmap Binarize(Bitmap source, byte threshold, bool parallel, int? dop = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        // 32 бит формат — стабильный stride, фиксированная схема BGRA.
        var width = source.Width;
        var height = source.Height;
        var dest = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        var rect = new Rectangle(0, 0, width, height);
        BitmapData? srcData = null, dstData = null;
        try
        {
            srcData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            dstData = dest.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            var srcStride = srcData.Stride;
            var dstStride = dstData.Stride;
            var srcPtr = (byte*)srcData.Scan0;
            var dstPtr = (byte*)dstData.Scan0;

            void ProcessRow(int y)
            {
                var srcRow = srcPtr + y * srcStride;
                var dstRow = dstPtr + y * dstStride;
                for (int x = 0; x < width; x++)
                {
                    var off = x * 4;
                    // BGRA, яркость по Rec.601
                    byte b = srcRow[off + 0];
                    byte g = srcRow[off + 1];
                    byte r = srcRow[off + 2];
                    int luma = (299 * r + 587 * g + 114 * b) / 1000;
                    byte v = luma > threshold ? (byte)255 : (byte)0;
                    dstRow[off + 0] = v;
                    dstRow[off + 1] = v;
                    dstRow[off + 2] = v;
                    dstRow[off + 3] = 255;
                }
            }

            if (parallel)
            {
                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = dop ?? Environment.ProcessorCount,
                };
                Parallel.For(0, height, options, ProcessRow);
            }
            else
            {
                for (int y = 0; y < height; y++) ProcessRow(y);
            }
        }
        finally
        {
            if (srcData != null) source.UnlockBits(srcData);
            if (dstData != null) dest.UnlockBits(dstData);
        }
        return dest;
    }
}
