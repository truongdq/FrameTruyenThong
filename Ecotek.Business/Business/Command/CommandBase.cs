using Ecotek.Common.SupperSocket;
using System.Collections.Generic;

namespace Ecotek.Business.Business.Command
{

    public abstract class CommandBase
    {
        public Server Server { set; get; }

        public virtual void SendMsg() { }

        public virtual void SendMsg(ClientSession session, byte[] data) { }

        public virtual void SendMsgAll(List<ClientSession> sessions) { }
    }

}