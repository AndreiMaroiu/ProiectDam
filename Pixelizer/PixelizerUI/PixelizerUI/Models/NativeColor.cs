using System.Diagnostics;

namespace PixelizerUI.Models
{
    [DebuggerDisplay("b: {Blue} g: {Green} r: {Red} a: {Alpha}")]
    public unsafe struct NativeColor
    {
        public byte Blue { get; set; }
        public byte Green { get; set; }
        public byte Red { get; set; }
        public byte Alpha { get; set; }

        public int ToInt()
        {
            int result = 0;
            byte* temp = (byte*)&result;

            temp[0] = Blue;
            temp[1] = Green;
            temp[2] = Red;
            temp[3] = Alpha;

            return result;
        }

        public static NativeColor FromInt(int* value)
        {
            byte* current = (byte*)value;

            return new NativeColor()
            {
                Blue = current[0],
                Green = current[1],
                Red = current[2],
                Alpha = current[3],
            };
        }
    }
}
