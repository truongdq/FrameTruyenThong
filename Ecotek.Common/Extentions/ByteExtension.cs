namespace Ecotek.Common.Extentions
{
    public static class ByteExtension
    {
        public static string ConvertToHex(this byte value)
        {
            return value.ToString("X");
        }
    }
}
