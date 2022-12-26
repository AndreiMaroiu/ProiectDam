using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelizerUI.Models
{
    [DebuggerDisplay("b: {Blue} g: {Green} r: {Red} a: {Alpha}")]
    internal unsafe struct NativeColor
    {
        public byte Blue { get; set; }
        public byte Green { get; set; }
        public byte Red { get; set; }
        public byte Alpha { get; set; }

        public int ToInt()
        {
            int result = 0;
            byte* temp = (byte*)&result;

            *temp++ = Blue;
            *temp++ = Green;
            *temp++ = Red;
            *temp++ = Alpha;

            return result;
        }

        public static NativeColor FromInt(int* value)
        {
            byte* current = (byte*)value;

            return new NativeColor()
            {
                Blue = *current++,
                Green = *current++,
                Red = *current++,
                Alpha = *current++,
            };
        }
    }
}
