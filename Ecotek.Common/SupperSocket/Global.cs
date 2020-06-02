namespace Ecotek.Common.SupperSocket
{
    public class Global
    {
        public static string g_SiteTitle = "Socket - SocketAsyncEventArgs";

        /// <summary>
        /// Command character 1
        /// </summary>
        public static char g_Division1 = '▦';
        /// <summary>
        /// Command character  2
        /// </summary>
        public static char g_Division2 = ',';

        /// <summary>
        /// Size of instruction
        /// </summary>
        public static int g_CommandSize = 1;
        /// <summary>
        /// Size of the message if the data size is large.
        /// Usually one message is 1024 bytes in size
        /// Since 4 bytes of header are required, it should be specified as 1002 bytes or less.
        /// (But let's test it for safe transmission. Recommended is 1000 bytes or less)
        /// If you have more than one extra character,.
        /// </summary>
        public static int g_CutByteSize = 995;
        /// <summary>
        /// If a separate header 1 is added to the data, the size of the header
        /// </summary>
        public static int g_DataHeader1Size = 10;
        /// <summary>
        /// When attaching a separate header 2 to the data, the size of the header
        /// </summary>
        public static int g_DataHeader2Size = 10;

        public static string ddMMyyyyHHmmss = "dd/MM/yyyy HH:mm:ss";
        public static string HHmmss = "HH:mm:ss";

        public static string Separator = "@_@";
        public static string FormatLog = $"[{0}]{Separator}[{1}]{Separator}[{2}]{Separator}{3}";
    }
}
