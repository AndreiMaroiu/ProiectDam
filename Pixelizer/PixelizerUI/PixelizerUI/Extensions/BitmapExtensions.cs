using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PixelizerUI.Models;
using System.Diagnostics;
using System.IO;

namespace PixelizerUI.Extensions
{
    internal static class BitmapExtensions
    {
        public static WriteableBitmap ToWritable(this Bitmap bitmap)
        {
            using MemoryStream memoryStream = new();
            bitmap.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return WriteableBitmap.Decode(memoryStream);
        }

        public static PixelMatrix GetPixelMatrix(this WriteableBitmap writeableBitmap)
        {
            using ILockedFramebuffer buffer = writeableBitmap.Lock();
            PixelFormat format = buffer.Format;
            Debug.Assert(format is PixelFormat.Bgra8888);

            return new(buffer.Address, buffer.Size);
        }
    }
}
