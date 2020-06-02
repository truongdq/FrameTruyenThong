using Ecotek.Common.Extentions;
using System;
using System.IO;
using System.Text;

namespace Ecotek.Common
{
    public class FileHelper
    {
        public const string Format = "{0}{1}{2}----------------------------{3}";
        public const string Log = "Log";
        public const string Error = "Error";
        public const string Utf8 = "utf-8";
        public const string FormatContent = "{0}:{1} - {2}";
        public const string HHmmss = "HH-mm-ss";
        public const string HHmm = "HH-mm";
        public const string ddMMyyyy = "dd-MM-yyyy";
        public const string FormatFilePath = "{0}{1}.txt";
        public const string FormatPath = @"{0}\{1}\";

        private static object objWrite = true;

        /// <summary>
        /// Ghi một nội dung ra file
        /// Kiểm tra xem thư mục đã có chưa, thư mục chưa có thì tạo thư mục
        /// Kiểm tra file đã tồn tại chưa, chưa tồn tại thì tạo file
        /// </summary>
        public static bool WriteFile(string pathWrite, string content)
        {
            var result = false;

            lock (objWrite)
            {
                // Tạo thư mục nếu như thư mục chưa tồn tại
                var folder = Path.GetDirectoryName(pathWrite);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                // Tạo file nếu như file chưa tồn tại
                if (!File.Exists(pathWrite))
                    using (var fs = new FileStream(pathWrite, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) fs.Close();

                // Thực hiện ghi file
                using (var sw = new StreamWriter(pathWrite, true, Encoding.GetEncoding(Utf8)))
                {
                    sw.WriteLine(content);
                    result = true;
                }
            }
            return result;
        }

        public static void WriteFile(string content, string folder, string logFileName = "")
        {
            WriteFile(DateTime.Now, content, folder, logFileName);
        }

        public static void WriteFile(DateTime time, string content, string folder, string logFileName = "")
        {
            try
            {
                var path = Path.Combine(FormatPath.Frmat(folder, time.ToString(ddMMyyyy)));

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var file = FormatFilePath.Frmat(path, logFileName.IsNull() ? time.ToString(HHmm) : logFileName);
                WriteFile(file, content);
            }
            catch { }
        }

        /// <summary>
        /// Ghi log
        /// </summary>
        /// <param name="logType">Tiêu đề Log</param>
        /// <param name="content">Nội dung</param>
        /// <param name="folder">Thư mục</param>
        /// <param name="logFileName">Tên file</param>
        /// <param name="isWriteLog">Có ghi log hay không (true: có, false: không)</param>
        public static void WriteLog(string logType, string content, string folder, string logFileName = "", bool isWriteLog = true)
        {
            if (!isWriteLog) return;

            WriteFile(FormatContent.Frmat(DateTime.Now.ToString(HHmmss), logType, content), string.Format("{0}/{1}", Log, folder), logFileName);
        }

        /// <summary>
        /// Ghi log lỗi
        /// </summary>
        /// <param name="logType">Tiêu đề Log</param>
        /// <param name="content">Nội dung</param>
        /// <param name="isWriteLog">Có ghi log hay không (true: có, false: không)</param>
        public static void WriteLogError(string logType, string content, bool isWriteLog = true)
        {
            WriteLog(logType, content, Error, string.Empty, isWriteLog);
        }
    }
}
