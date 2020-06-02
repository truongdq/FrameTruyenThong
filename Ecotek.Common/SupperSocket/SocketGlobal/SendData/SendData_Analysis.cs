namespace Ecotek.Common.SupperSocket
{
    /// <summary>
    /// A class that replaces the original data with the command ep.
    /// </summary>
    public class SendData_Analysis
    {
        /// <summary>
        /// Source data before analysis
        /// </summary>
        public DataOriginal OriginalData { get; set; }

        /// <summary>
        /// Data from which the original data was analyzed
        /// </summary>
        public DataSend DataSend;

        /// <summary>
        /// String type of separated data
        /// </summary>
        public string Data;
        /// <summary>
        /// Data cut into one-step separator of separated data
        /// </summary>
        public string[] Datas;

        public byte[] DataReceive;

        public SendData_Analysis()
        {
        }

        public SendData_Analysis(byte[] byteOri)
        {
            //Save original
            DataOriginal dataOri = new DataOriginal();
            dataOri.Data = byteOri;
            this.OriginalData = dataOri;

            //Convert to class
            DataSend = new DataSend(this.OriginalData);
        }

        public SendData_Analysis(DataOriginal insDataOri)
        {
            //Save original
            this.OriginalData = insDataOri;

            //Convert to class
            DataSend = new DataSend(this.OriginalData);
        }

        /// <summary>
        /// Cut the separated data into single-step delimiters.
        /// </summary>
        public void CutData1ToDatas()
        {
            DataOriginal dataOri = new DataOriginal();
            dataOri.Data = this.DataSend.Data_Byte;
            this.DataReceive = dataOri.Data;
        }

        public void BuilData(byte[] data)
        {

        }
    }
}
