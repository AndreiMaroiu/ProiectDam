using Avalonia;
using Avalonia.Media.Imaging;
using PixelizerUI.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PixelizerUI.Models
{
    internal class Pixelizer
    {
        private readonly int _factor;

        private PixelMatrix _matrix;
        private PixelMatrix _resultMatrix;

        private Action<uint, uint, uint, uint> _strategy;

        public Pixelizer(int factor)
        {
            _factor = factor;
        }

        public async Task<PixelizeResult> PixelizeAsync(Bitmap inputImage, PixelizeStrategy strategy)
        {
            //if (CanPixelize(inputImage) is false)
            //{
            //    return PixelizeResult.Failed;
            //}

            WriteableBitmap writeableBitmap = inputImage.ToWritable();
            _matrix = writeableBitmap.GetPixelMatrix();

            WriteableBitmap result = new(new PixelSize(inputImage.PixelSize.Width / _factor, inputImage.PixelSize.Height / _factor), inputImage.Dpi);
            _resultMatrix = result.GetPixelMatrix();

            uint height = (uint)inputImage.PixelSize.Height;
            uint width = (uint)inputImage.PixelSize.Width;

            SetStrategy(strategy);

            Task[] tasks = new Task[4];

            tasks[0] = Task.Run(() => PixelizeBlock(0, height / 2, 0, width / 2));
            tasks[1] = Task.Run(() => PixelizeBlock(0, height / 2, width / 2, width));
            tasks[2] = Task.Run(() => PixelizeBlock(height / 2, height, 0, width / 2));
            tasks[3] = Task.Run(() => PixelizeBlock(height / 2, height, width / 2, width));

            await Task.WhenAll(tasks);

            return new PixelizeResult()
            {
                Result = result,
                UnscaledResult = writeableBitmap,
            };
        }

        private bool CanPixelize(Bitmap image)
        {
            return image.PixelSize.Width % _factor == 0 && image.PixelSize.Height % _factor == 0;
        }

        private void PixelizeBlock(uint startRow, uint rowEndPoint, uint startColumn, uint columnEndPoint)
        {
            uint blockSize = (uint)_factor;

            for (uint row = startRow, endRow = startRow + blockSize; row < rowEndPoint; row += blockSize, endRow += blockSize)
            {
                for (uint column = startColumn, endColumn = startColumn + blockSize; column < columnEndPoint; column += blockSize, endColumn += blockSize)
                {
                    _strategy(row, endRow, column, endColumn);
                }
            }
        }

        private void AverageStrategy(uint row, uint endRow, uint column, uint endColumn)
        {
            uint blockSize = (uint)_factor;

            var color = _matrix.AverageColor(row, endRow, column, endColumn);

            _resultMatrix[row / blockSize, column / blockSize] = color;
        }

        private void MostCommonStrategy(uint row, uint endRow, uint column, uint endColumn)
        {
            uint blockSize = (uint)_factor;

            var color = _matrix.MostCommonColor(row, endRow, column, endColumn);

            _resultMatrix[row / blockSize, column / blockSize] = color;
        }

        private void Dither(uint startRow, uint endRow, uint startColumn, uint endColumn)
        {
            for (uint i = startRow; i < endRow; i++)
            {
                for (uint j = startColumn; j < endColumn; j++)
                {
                    uint row = i % 16;
                    uint column = j % 16;

                    NativeColor color = _matrix[i, j];
                    int value = BAYER_PATTERN_16X16[column, row];

                    NativeColor result = new()
                    {
                        Red = color.Red > value ? byte.MaxValue : byte.MinValue,
                        Green = color.Green > value ? byte.MaxValue : byte.MinValue,
                        Blue = color.Blue > value ? byte.MaxValue : byte.MinValue,
                        Alpha = byte.MaxValue,
                    };

                    _matrix[i, j] = result;

                    
                }
            }
        }

        private void SetStrategy(PixelizeStrategy strategy)
        {
            switch (strategy)
            {
                case PixelizeStrategy.Average:
                    _strategy = AverageStrategy;
                    break;
                case PixelizeStrategy.MostCommon:
                    _strategy = MostCommonStrategy;
                    break;
                case PixelizeStrategy.BayerDither:
                    _strategy = Dither;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        private static readonly int[, ] BAYER_PATTERN_16X16 =   
            {   //  16x16 Bayer Dithering Matrix.  Color levels: 256
                {     0, 191,  48, 239,  12, 203,  60, 251,   3, 194,  51, 242,  15, 206,  63, 254  }, 
                {   127,  64, 175, 112, 139,  76, 187, 124, 130,  67, 178, 115, 142,  79, 190, 127  },
                {    32, 223,  16, 207,  44, 235,  28, 219,  35, 226,  19, 210,  47, 238,  31, 222  },
                {   159,  96, 143,  80, 171, 108, 155,  92, 162,  99, 146,  83, 174, 111, 158,  95  },
                {     8, 199,  56, 247,   4, 195,  52, 243,  11, 202,  59, 250,   7, 198,  55, 246  },
                {   135,  72, 183, 120, 131,  68, 179, 116, 138,  75, 186, 123, 134,  71, 182, 119  },
                {    40, 231,  24, 215,  36, 227,  20, 211,  43, 234,  27, 218,  39, 230,  23, 214  },
                {   167, 104, 151,  88, 163, 100, 147,  84, 170, 107, 154,  91, 166, 103, 150,  87  },
                {     2, 193,  50, 241,  14, 205,  62, 253,   1, 192,  49, 240,  13, 204,  61, 252  },
                {   129,  66, 177, 114, 141,  78, 189, 126, 128,  65, 176, 113, 140,  77, 188, 125  },
                {    34, 225,  18, 209,  46, 237,  30, 221,  33, 224,  17, 208,  45, 236,  29, 220  },
                {   161,  98, 145,  82, 173, 110, 157,  94, 160,  97, 144,  81, 172, 109, 156,  93  },
                {    10, 201,  58, 249,   6, 197,  54, 245,   9, 200,  57, 248,   5, 196,  53, 244  },
                {   137,  74, 185, 122, 133,  70, 181, 118, 136,  73, 184, 121, 132,  69, 180, 117  },
                {    42, 233,  26, 217,  38, 229,  22, 213,  41, 232,  25, 216,  37, 228,  21, 212  },
                {   169, 106, 153,  90, 165, 102, 149,  86, 168, 105, 152,  89, 164, 101, 148,  85  }
            };
    }
}
