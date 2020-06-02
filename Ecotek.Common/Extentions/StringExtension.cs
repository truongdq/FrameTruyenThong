using System;

namespace Ecotek.Common.Extentions
{
    public static class StringExtension
    {
        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string Frmat(this string str, params object[] @params)
        {
            return string.Format(str, @params);
        }

        public static Guid ConvertToGuid(this string str)
        {
            return new Guid(str);
        }

        public static byte ToByte(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;

            return Convert.ToByte(str);
        }

        public static bool IsNumber(this string str)
        {
            try
            {
                return byte.TryParse(str, out byte result);
            }
            catch
            {
                return false;
            }
        }

        public static string ConvertToPort(this string str)
        {
            return int.Parse(str, System.Globalization.NumberStyles.HexNumber).ToString();
        }

        public static byte[] ToByteArray(this string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
    }
}
