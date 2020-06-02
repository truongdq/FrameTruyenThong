using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Ecotek.Common
{
    public static class Utils
    {
        /// <summary>
        /// Remove diacritic characters from the input text, this version work well on latin font only
        /// </summary>
        /// <param roomID="InString"></param>
        /// <returns></returns>
        public static string LatinToAscii(string InString)
        {
            string charString;
            char ch;
            int charsCopied;
            StringBuilder newString = new StringBuilder();

            for (int i = 0; i < InString.Length; i++)
            {
                charString = InString.Substring(i, 1);
                charString = charString.Normalize(NormalizationForm.FormKD);
                // If the character doesn't decompose, leave it as-is

                if (charString.Length == 1)
                    newString.Append(charString);
                //newString += charString;
                else
                {
                    charsCopied = 0;
                    for (int j = 0; j < charString.Length; j++)
                    {
                        ch = charString[j];
                        // If the char is 7-bit ASCII, add

                        if (ch < 128)
                        {
                            newString.Append(ch);
                            //newString += ch;
                            charsCopied++;
                        }
                    }
                    /* If we've decomposed non-ASCII, give it back
                     * in its entirety, since we only mean to decompose
                     * Latin chars.
                    */
                    if (charsCopied == 0)
                        newString.Append(InString.Substring(i, 1));
                    //newString += InString.Substring(i, 1);
                }
            }
            return newString.ToString();
        }
        public static T ReadObject<T>(this IDataReader reader, Func<IDataRecord, T> creator) where T : class
        {
            if (reader.Read())
            {
                return
                    creator(reader);
            }
            return null;
        }

        public static IEnumerable<T> ReadListObject<T>(this IDataReader reader, Func<IDataRecord, T> creator)
        {
            var collection = new List<T>();
            while (reader.Read())
                collection.Add(creator(reader));
            return collection;
        }

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        public static byte[] StructureToByteArray<T>(T obj) where T : struct
        {
            int nLen = Marshal.SizeOf(obj);
            byte[] array = new byte[nLen];
            IntPtr ptr = Marshal.AllocHGlobal(nLen);
            try
            {
                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, array, 0, nLen);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return array;
        }

        public static bool hasStartFrame(ArraySegment<byte> pBuffer)
        {
            return
                BitConverter.ToUInt16(pBuffer.Array, pBuffer.Offset) == 0xD8FF;
        }

        public static bool hasFinishFrame(ArraySegment<byte> pBuffer)
        {
            return
                BitConverter.ToUInt16(pBuffer.Array, pBuffer.Offset + pBuffer.Count - 3) == 0xD9FF;
        }

        // Unit Test: Done! 
        // Không kiểm tra dạng số nhập vào ở  phút, giây có >=0 và <60 không vì không cần thiết 
        public static byte CheckSumThuong(byte[] inputArray, byte Cmd, int lenData, int iStartIndex, int iEndIndex)
        {
            int tempValue = 0;
            tempValue = Cmd;
            for (int i = iStartIndex; i <= iEndIndex; i++)
                tempValue += inputArray[i];
            return Convert.ToByte((0 - (~(tempValue & 0xFF))) & 0xFF);
        }

        public static byte Checksum(byte[] pData, int iSartIndex, int iStopIndex)
        {
            int nChecksum = 0;
            int i = 0;
            for (i = iSartIndex; i < iStopIndex + 1; i++)
            {
                nChecksum += pData[i];
            }
            byte result = (byte)(0 - (~((byte)(nChecksum))));

            return result;
        }

        public static string Decode(string strInput)
        {
            if (strInput.Length <= 2)
                return "";

            string strOutput = "";
            foreach (char c in strInput)
            {
                byte[] i = System.Text.Encoding.Unicode.GetBytes(c.ToString());
                i[0] = (byte)(i[0] - 123);
                strOutput = strOutput + System.Text.Encoding.Unicode.GetString(i);
            }
            return strOutput.Substring(1, strOutput.Length - 2);
        }
        public static string Encode(string xauCanMaHoa)
        {
            string xauDaMaHoa = "";
            xauCanMaHoa = "3" + xauCanMaHoa + "3";
            foreach (char c in xauCanMaHoa)
            {
                byte[] i = System.Text.Encoding.Unicode.GetBytes(c.ToString());
                i[0] = (byte)(i[0] + 123);
                xauDaMaHoa = xauDaMaHoa + System.Text.Encoding.Unicode.GetString(i);
            }
            return xauDaMaHoa;
        }
    }
}
