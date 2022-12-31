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
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}
