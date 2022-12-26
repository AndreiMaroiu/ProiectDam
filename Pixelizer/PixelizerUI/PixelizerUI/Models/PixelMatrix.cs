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

        public int* this[int i, int j] => _array + (i * _size.Width + j);
    }
}
