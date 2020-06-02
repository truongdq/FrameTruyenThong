namespace Ecotek.Common.SupperSocket
{
    /// <summary>
    /// Class for receiving large data
    /// </summary>
    public class LargeData_GetData
    {
        /// <summary>
        /// Indexes from DBI
        /// </summary>
        public int DBIndex { get; set; }
        /// <summary>
        /// Received data
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Size of data received so far
        /// </summary>
        public int Length_Now { get; set; }

        /// <summary>
        /// Total data size to be received
        /// </summary>
        public int Length_Total { get; set; }

        /// <summary>
        /// Number of data received so far
        /// </summary>
        public int Count_Now { get; set; }
        /// <summary>
        /// Total number of data to be received
        /// </summary>
        public int Count_Total { get; set; }

        /// <summary>
        /// Large data data classification
        /// </summary>
        public TypeLargeData TypeLargeData { get; set; }

        /// <summary>
        /// File name to save
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Path to Save
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// Whether or not the file was uploaded and finished.
        /// Used to prevent duplicate completion processing.
        /// </summary>
        public bool DataCompleteRead { get; set; }

        /// <summary>
        /// Set the data to be passed through this class.
        /// </summary>
        public void SettingData(TypeLargeData typeLD, string sFileName, int nTotalLength, int nTotalCount, int nDBIndex)
        {
            TypeLargeData = typeLD;
            FileName = sFileName;

            Length_Total = nTotalLength;
            Length_Now = 0;
            //Hold data space.
            Data = new byte[this.Length_Total];

            Count_Total = nTotalCount;
            Count_Now = 0;
            DataCompleteRead = false;

            DBIndex = nDBIndex;
        }
    }
}
