using Ecotek.Data.Data;
using System.Collections.Generic;
using System.Linq;

namespace Ecotek.Business.Business
{
    public class XeBusiness : IXeBusiness
    {
        private readonly IXeData _iXeData;

        public XeBusiness()
        {
            _iXeData = new XeData();
        }

        public List<Xe> GetXeList()
        {
            return _iXeData.GetXeList().Select(x => new Xe()
            { 
                ID_Xe = x.ID_Xe,
                So_Xe = x.So_Xe,
                Trang_Thai = x.Trang_Thai,
                Ghi_Chu = x.Ghi_Chu,
                ID_LB_HX = x.ID_LB_HX,
                IP_Address = x.IP_Address
            }).ToList();
        }
    }
}
