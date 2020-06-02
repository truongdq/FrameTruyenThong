namespace Ecotek.Common.SupperSocket
{
    public class SocketUtile : claEncoding
    {
        private SocketUtile() { }

        public static SocketUtile Inst { get; } = new SocketUtile();

        /// <summary>
        /// Creates a header with the specified number to match the size of 'g_DataHeader1Size'.
        /// </summary>
        /// <param name="intData"></param>
        /// <returns></returns>
        public byte[] IntToByte(int intData)
        {
            return StringToByte(string.Format("{0:D10}", intData.GetHashCode()));
        }

        /// <summary>
        /// Replaces the specified byte array with a number.
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public int ByteToInt(byte[] byteData)
        {
            return claNumber.StringToInt(ByteToString(byteData));
        }
    }
}
