using System.Collections.Generic;

namespace Ecotek.Business
{
    public class Data0x0A
    {
        public int Index { set; get; } = 0;

        public List<Frame> Frames { set; get; } = new List<Frame>();

        public ThongSoFrame ThongSoFrame { set; get; } = new ThongSoFrame();
    }
}
