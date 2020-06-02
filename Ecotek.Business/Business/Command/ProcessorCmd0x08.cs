using Ecotek.Common;
using Ecotek.Common.SupperSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecotek.Business.Business.Command
{
    public class ProcessorCmd0x08 : CommandBase
    {
        public override void SendMsgAll(List<ClientSession> sessions)
        {
            if (Server != null)
            {
                Frame frame = new Frame()
                {
                    Code = Constants.Hx08,
                    Data = new byte[] { Constants.Hx00 }
                };

                Parallel.ForEach(sessions, x =>
                {
                    Server.SendMsg(x, frame.Get());
                });
            }    
        }
    }
}
