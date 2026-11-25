using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PixelLab14.Core;

/// <summary>
/// Изображение в виде массива байтов BGRA (по 4 байта на пиксель).
/// Отдельный массив позволяет потокам менять пиксели без доступа к Bitmap/GUI.
/// </summary>
internal sealed class PixelImage
{
    // Размер изображения в пикселях.
    public int Width { get; }
    public int Height { get; }
    // Плотный массив BGRA: индекс пикселя (y * Width + x) * 4.
    public byte[] Pixels { get; }

    public PixelImage(int width, int height, byte[] pixels)
    {
        // Проверяем, что буфер действительно содержит четыре байта на пиксель.
        if (width <= 0 || height <= 0 || pixels.Length != checked(width * height * 4))
            throw new ArgumentException("Некорректный размер пиксельного буфера.");

        Width = width;
        Height = height;
        Pixels = pixels;
    }

    /// <summary>Декодирование файла. Вызывается из фоновой задачи, не из UI.</summary>
    public static PixelImage Decode(byte[] encoded)
    {
        // Поток остаётся открытым до завершения декодирования Image.
        using var stream = new MemoryStream(encoded, writable: false);
        using var original = Image.FromStream(stream, useEmbeddedColorManagement: true, validateImageData: true);
        // Приводим любые поддерживаемые форматы к 32bpp ARGB.
        using var normalized = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(normalized))
        {
            g.Clear(Color.Transparent);
            g.DrawImage(original, 0, 0, original.Width, original.Height);
        }

        return FromBitmap(normalized);
    }

    private static PixelImage FromBitmap(Bitmap bitmap)
    {
        var width = bitmap.Width;
        var height = bitmap.Height;
        var pixels = new byte[checked(width * height * 4)];
        var rectangle = new Rectangle(0, 0, width, height);
        // LockBits открывает доступ к памяти Bitmap без медленных вызовов GetPixel.
        var data = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        try
        {
            // Stride учитывается отдельно: строки Bitmap могут содержать отступы.
            for (var y = 0; y < height; y++)
                Marshal.Copy(IntPtr.Add(data.Scan0, y * data.Stride), pixels, y * width * 4, width * 4);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
        return new PixelImage(width, height, pixels);
    }

    /// <summary>Создание Bitmap из нашего готового массива пикселей.</summary>
    public Bitmap ToBitmap()
    {
        var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
        var rectangle = new Rectangle(0, 0, Width, Height);
        // Обратное копирование массива в память Bitmap для просмотра и сохранения.
        var data = bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        try
        {
            for (var y = 0; y < Height; y++)
                Marshal.Copy(Pixels, y * Width * 4, IntPtr.Add(data.Scan0, y * data.Stride), Width * 4);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
        return bitmap;
    }
}
