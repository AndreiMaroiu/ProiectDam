using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelizerUI.Models
{
    internal unsafe class PixelMatrix
    {
        private readonly int* _array;
        private readonly PixelSize _size;

        public PixelMatrix(IntPtr array, PixelSize pixelSize)
        {
            _array = (int*)array;
            _size = pixelSize;
        }

        public NativeColor this[uint i, uint j]
        {
            get => NativeColor.FromInt(_array + (i * _size.Width + j));
            set => *(_array + (i * _size.Width + j)) = value.ToInt();
        }

        public NativeColor this[int i, int j]
        {
            get => NativeColor.FromInt(_array + (i * _size.Width + j));
            set => *(_array + (i * _size.Width + j)) = value.ToInt();
        }

        public void AverageColor (uint startRow, uint endRow, uint startColumn, uint endColumn)
        {
            ulong red = 0;
            ulong green = 0;
            ulong blue = 0;
            ulong alpha = 0;

            uint count = 0;

            for (uint i = startRow; i < endRow; i++)
            {
                for (uint j = startColumn; j < endColumn; j++)
                {
                    NativeColor current = this[i, j];

                    red += current.Red;
                    green += current.Green;
                    blue += current.Blue;
                    alpha += current.Alpha;

                    count++;
                }
            }

            var average = new NativeColor()
            {
                Red = (byte)(red / count),
                Green = (byte)(green / count),
                Blue = (byte)(blue / count),
                Alpha = (byte)(alpha / count)
            };

            for (uint i = startRow; i < endRow; i++)
            {
                for (uint j = startColumn; j < endColumn; j++)
                {
                    this[i, j] = average;
                }
            }
        }
    }
}
