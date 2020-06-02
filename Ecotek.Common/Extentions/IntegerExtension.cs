namespace Ecotek.Common.Extentions
{
    public static class IntegerExtension
    {
        public static int LengthLow(this int vaule)
        {
            return vaule & 0xFF;
        }

        public static int LengthHigh(this int value)
        {
            return value >> 8;
        }

        public static string ConvertToHex(this int value)
        {
            return value.ToString("X");
        }
    }
}
