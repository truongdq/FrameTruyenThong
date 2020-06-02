namespace Ecotek.Business
{
    public class Data0x09
    {
        #region Properties
        public byte[] IP_ECR { set; get; } = new byte[4];

        public byte[] IP_Server { set; get; } = new byte[4];

        public byte[] Port_Server { set; get; } = new byte[2];

        public byte SSID_Length { set; get; }

        public byte[] SSID { set; get; } = new byte[32];

        public byte Key_Length { set; get; }

        public byte[] Key { set; get; } = new byte[16];

        public byte Mode_ECR { set; get; }

        public byte[] IP_Server_ECR { set; get; } = new byte[4];

        public byte[] Port_Server_ECR { set; get; } = new byte[2];

        public int Length
        {
            get
            {
                return IP_ECR.Length + IP_Server.Length + Port_Server.Length + 3 + SSID.Length + Key.Length + IP_Server_ECR.Length + Port_Server_ECR.Length;
            }
        }
        #endregion
    }
}
