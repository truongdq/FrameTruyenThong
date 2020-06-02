using Ecotek.Common;
using Ecotek.Common.Extentions;

namespace Ecotek.Business
{
    public class Frame
    {
        #region Properties
        public byte Header1 { set; get; } = Constants.HxAA;

        public byte Header2 { set; get; } = Constants.Hx55;

        public byte Header3 { set; get; } = Constants.HxCC;

        public byte Code { set; get; }

        public byte LengthLow
        {
            get
            {
                return (byte)(Data == null ? Constants.Hx00 : Data.Length.LengthLow());
            }
        }

        public byte LengthHigh
        {
            get
            {
                return (byte)(Data == null ? Constants.Hx00 : Data.Length.LengthHigh());
            }
        }

        public byte[] Data { set; get; }

        public byte CheckSum
        {
            get
            {
                int checksum = 0;
                checksum += Code + LengthLow + LengthHigh;
                for (int i = 0; i < Data?.Length; i++)
                {
                    checksum += Data[i];
                }

                return (byte)(0 - (~(byte)checksum));
            }
        }
        #endregion

        public byte[] Get()
        {
            int lengthData = 0;
            int capacity = 7;

            if (Data != null) lengthData = Data.Length;

            byte[] result = new byte[capacity + lengthData];
            result[0] = Header1;
            result[1] = Header2;
            result[2] = Header3;
            result[3] = Code;
            result[4] = LengthLow;
            result[5] = LengthHigh;
            if (lengthData > 0)
            {
                for (int i = 6; i < capacity + lengthData - 1; i++)
                {
                    result[i] = Data[i - 6];
                }
            }
            result[capacity + lengthData - 1] = CheckSum;

            return result;
        }
    }
}
