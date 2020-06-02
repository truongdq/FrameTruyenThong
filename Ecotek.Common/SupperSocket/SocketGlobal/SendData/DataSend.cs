using System;
using System.Collections.Generic;
using System.Text;

namespace Ecotek.Common.SupperSocket
{
    /// <summary>
    /// A class that holds data before conversion to bytes before sending and receiving messages.
    /// </summary>
    public class DataSend
    {
        /// <summary>
        /// 명령어
        /// </summary>
        public Command Command { get; set; }

        public eOpcode eCommand { get; set; }
        /// <summary>
        /// Data (byte []), priority 1.
        /// </summary>
        public byte[] Data_Byte { get; set; }
        /// <summary>
        ///Data (string), priority 3.
        /// String List of data converted to a string.
        /// </summary>
        public string Data_String { get; set; }

        /// <summary>
        /// Data (List), priority 2.
        /// </summary>
        public List<string> Data_List { get; set; }

        private void ResetClass()
        {
            Data_List = new List<string>();
        }

        public DataSend()
        {
            ResetClass();

            this.Command = Command.None;
            this.Data_String = "";
        }

        public DataSend(DataOriginal dataOri)
        {
            ResetClass();

            this.DataOriginalToThis(dataOri);
        }

        public DataSend(Command typeCommand, params string[] sDatas)
        {
            ResetClass();

            this.Command = typeCommand;

            foreach (string sTemp in sDatas)
            {
                this.Data_List.Add(sTemp);
            }

            // Convert the input data.
            this.CreateDataSend();

        }


        public DataSend(params byte[] sDatas)
        {
            ResetClass();

            //Converts the input data.
            this.CreateDataSendEx(sDatas);

        }

        /// <summary>
        /// Make the contents of the specified list one line by using delimiter.
        /// </summary>
        private string ListToString(List<string> listString, string sDivision)
        {
            if (0 >= listString.Count)
            {   //If this list and give details empty string.
                return null;
            }

            //If the list with the content using a separator made of a single line.
            StringBuilder sbReturn = new StringBuilder();
            //It makes the contents of the list in a row.
            foreach (string sFor in listString)
            {
                sbReturn.Append(sFor);
                sbReturn.Append(sDivision);
            }

            return sbReturn.ToString();
        }

        /// <summary>
        /// Change command to byte format.
        /// </summary>
        /// <returns></returns>
        private byte[] ByteToCommand()
        {
            //The size of this box is determined by the 'claGlobal.g_CommandSize'
            return SocketUtile.Inst.StringToByte(string.Format("{0:D4}", this.Command.GetHashCode()));
        }

        /// <summary>
        /// It makes the original data in this class.
        /// </summary>
        /// <param name="dataOri"></param>
        public void DataOriginalToThis(DataOriginal dataOri)
        {
            DataOriginalToThis(dataOri.Data);
        }

        private void DataOriginalToThis(byte[] byteOri)
        {
            byte[] byteTemp;

            //Cut and paste commands.
            byteTemp = new byte[Global.g_CommandSize];
            //Coppy buffer
            Buffer.BlockCopy(byteOri, 0, byteTemp, 0, Global.g_CommandSize);

            //Converted into a command
            this.Command = (Command)(SocketUtile.Inst.ByteToInt(byteTemp));

            //Coppy Data
            this.Data_Byte = new byte[byteOri.Length];
            Buffer.BlockCopy(byteOri, 0, Data_Byte, 0, byteOri.Length);
        }

        public DataOriginal CreateDataOriginal()
        {
            if ((null == Data_Byte) || (0 >= Data_Byte.Length))
            {
                return CreateDataOriginal_String();
            }

            return CreateDataOriginal_Byte();
        }


        private DataOriginal CreateDataOriginal_String()
        {
            //It converts an input data.
            CreateDataSend();

            //And the number of bytes processed.
            return CreateDataOriginal_Byte();
        }

        private DataOriginal CreateDataOriginal_Byte()
        {
            //Send your return to the original data for
            DataOriginal dataReturn = new DataOriginal();
            //Secure data space
            dataReturn.Data = new byte[Global.g_CommandSize + this.Data_Byte.Length];
            //Coppy Data
            Buffer.BlockCopy(Data_Byte, 0, dataReturn.Data, 0, Data_Byte.Length);

            return dataReturn;
        }

        /// <summary>
        /// It converts the input data into a one-line data.
        /// </summary>
        private void CreateDataSend()
        {
            //To convert the string to a list of one-line string.
            string sResult = ListToString(Data_List, Global.g_Division1.ToString());

            if (null != sResult)
            {   
                // Write a list of results only when there is information in the list.
                Data_String = sResult;
            }

            //It allows converting the data into a byte string.
            Data_Byte = SocketUtile.Inst.StringToByte(Data_String);
        }

        private void CreateDataSendEx(byte[] Data_Byte)
        {
            this.Data_Byte = Data_Byte;
        }
    }
}
