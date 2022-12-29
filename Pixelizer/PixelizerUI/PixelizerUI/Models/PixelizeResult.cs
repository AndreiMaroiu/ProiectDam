using Avalonia.Media.Imaging;

namespace PixelizerUI.Models
{
    internal class PixelizeResult
    {
        public Bitmap Result { get; init; }
        public Bitmap UnscaledResult { get; init; }

        public bool IsSuccesul => Result is not null;

        public static PixelizeResult Failed { get; } = new PixelizeResult();
    }
}
