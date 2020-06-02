namespace Ecotek.Common.Extentions
{
    public static class ByteArrayExtension
    {
        public static string ConvertToString(this byte[] buffer)
        {
            return System.Text.Encoding.UTF8.GetString(buffer);
        }

        public static string UnsafeAsciiBytesToString(this byte[] buffer, int offset)
        {
            int end = offset;
            while (end < buffer.Length && buffer[end] != 0)
            {
                end++;
            }
            unsafe
            {
                fixed (byte* pAscii = buffer)
                {
                    return new string((sbyte*)pAscii, offset, end - offset);
                }
            }
        }
    }
}
