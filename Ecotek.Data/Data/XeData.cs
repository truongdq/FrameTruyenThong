using Ecotek.Common;
using System.Collections.Generic;

namespace Ecotek.Data.Data
{
    public class XeData : IXeData
    {
        public List<Xe> GetXeList()
        {
            return SqlManager.ReadEntityList<Xe>("SELECT ID_Xe, [So_Xe], [Trang_Thai],[Ghi_Chu],[ID_LB_HX],[IP_Address] FROM [dbo].[tblXe]",
                                                 System.Data.CommandType.Text);
        }
    }
}
