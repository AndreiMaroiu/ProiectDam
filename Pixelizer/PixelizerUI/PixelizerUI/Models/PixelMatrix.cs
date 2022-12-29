﻿using Avalonia;
using System;

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

        public int GetIntAt(uint i, uint j)
            => *(_array + (i * _size.Width + j));

        public void SetIntAt(int value, uint i, uint j)
            => *(_array + (i * _size.Width + j)) = value;

        public NativeColor AverageColor(uint startRow, uint endRow, uint startColumn, uint endColumn)
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

            NativeColor average = new NativeColor()
            {
                Red = (byte)(red / count),
                Green = (byte)(green / count),
                Blue = (byte)(blue / count),
                Alpha = (byte)(alpha / count)
            };

            int averageAsInt = average.ToInt();

            for (uint i = startRow; i < endRow; i++)
            {
                for (uint j = startColumn; j < endColumn; j++)
                {
                    SetIntAt(averageAsInt, i, j);
                }
            }

            return average;
        }
    }
}
