using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecotek.Business
{
    public class Xe
    {
        public int ID_Xe { set; get; }

        public string So_Xe { set; get; }

        public bool Trang_Thai { set; get; }

        public string Ghi_Chu { set; get; }

        public int ID_LB_HX { set; get; }

        public string IP_Address { set; get; }

        public bool IsOnline { set; get; }
    }
}
