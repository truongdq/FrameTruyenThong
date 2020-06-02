namespace Ecotek.Business
{
    public class ThongSoFrame
    {
        public string SessionID { get; set; }

        public int FrameSendCount { get; set; }

        public int FrameReceiveCount { get; set; }

        public int FrameTotal { get; set; }

        public string State { get; set; }
    }
}
