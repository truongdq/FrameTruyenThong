using Ecotek.Common.Extentions;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecotek.Common.SupperSocket
{
    public delegate void dgMessage(ClientSession session, LocalMessageEventArgs e);

    public class Server : AppServer<ClientSession, BinaryRequestInfo>
    {
        private const string FolderName = @"Command\{0}";

        public delegate void dgShowLog(string content);

        #region Events
        /// <summary>
        /// Content to be displayed in the log
		/// </summary>
		public event dgMessage OnReceived;

        /// <summary>
        /// Show UI messages
        /// </summary>
        public event dgMessage OnMessaged;

        public event dgShowLog OnShowLog;
        #endregion

        #region Properties
        /// <summary>
        /// Có ghi log hay không
        /// true: có, false: không
        /// </summary>
        public bool IsShowLog { set; get; }

        /// <summary>
        /// Thời gian timeout của mỗi session (đơn vị: giây)
        /// </summary>
        public int Timeout { set; get; } = 12;

        public List<ClientSession> ClientSessions { set; get; } = new List<ClientSession>();
        #endregion

        public Server() : base(new DefaultReceiveFilterFactory<ReceiveFilter, BinaryRequestInfo>())
        {
        }

        /// <summary>
        /// Occurs when a new session is accessed
        /// </summary>
        protected override void OnNewSessionConnected(ClientSession session)
        {
            if (IsShowLog)
            {
                string time = DateTime.Now.ToString(Global.HHmmss);
                string ip = session.RemoteEndPoint.Address.ToString();
                string sessionID = session.SessionID;

                //string log = string.Format("IP({0}_{1}_{2}): Đã kết nối", session.RemoteEndPoint.Address.ToString(), session.SessionID, time);
                //string log = Global.FormatLog.Frmat(time, ip, sessionID, "Đã kết nối");// string.Format("[{0}] [{1}] [{2}] Đã kết nối", time, ip, sessionID);
                string log = $"[{time}]{Global.Separator}[{ip}]{Global.Separator}[{sessionID}]{Global.Separator}Đã kết nối";
                OnShowLog(log);
                log = string.Format("({0}): Đã kết nối", time);
                FileHelper.WriteLog("", log, string.Format(FolderName, ip), sessionID);
            }

            // Events for messages to receive from the session
            session.OnMessaged += session_OnMessaged;

            base.OnNewSessionConnected(session);
            Timer timer = new Timer(OnTimeout, session, 0, 1000);
        }

        private void session_OnMessaged(ClientSession session, LocalMessageEventArgs e)
        {
            if (IsShowLog)
            {
                string time = DateTime.Now.ToString(Global.HHmmss);
                string ip = session.RemoteEndPoint.Address.ToString();
                string sessionID = session.SessionID;
                string message = BitConverter.ToString(e.bMessage);

                //string log = string.Format("IP({0}_{1}_{2})_OnMessaged_{3}", session.RemoteEndPoint.Address.ToString(), session.SessionID, time, e.Message);
                //string log = Global.FormatLog.Frmat(time, ip, sessionID, "OnMessaged " + message);//string.Format("[{0}] [{1}] [{2}] OnMessaged {3}", time, ip, sessionID, message);
                string log = $"[{time}]{Global.Separator}[{ip}]{Global.Separator}[{sessionID}]{Global.Separator}OnMessaged {message}";
                OnShowLog(log);
                log = string.Format("({0})_OnMessaged_{1}", time, message);
                FileHelper.WriteLog("", log, string.Format(FolderName, ip), sessionID);
            }

            OnMessaged(session, e);
        }

        /// <summary>
        /// I received the data!
        /// </summary>
        protected override void ExecuteCommand(ClientSession session, BinaryRequestInfo requestInfo)
        {
            // Display the message in the UI.
            LocalMessageEventArgs e = new LocalMessageEventArgs((requestInfo.Body), TypeLocal.None);

            if (IsShowLog)
            {
                if (e.bMessage[3] != Constants.Hx01)
                {
                    string time = DateTime.Now.ToString(Global.HHmmss);
                    string ip = session.RemoteEndPoint.Address.ToString();
                    string sessionID = session.SessionID;
                    string message = BitConverter.ToString(e.bMessage);

                    //string log = string.Format("IP({0}_{1}_{2})_RESPONSE_{3}", session.RemoteEndPoint.Address.ToString(), session.SessionID, time, BitConverter.ToString(e.bMessage));
                    //string log = Global.FormatLog.Frmat(time, ip, sessionID, "RESPONSE " + message); //string.Format("[{0}] [{1}] [{2}] RESPONSE {3}", time, ip, sessionID, message);
                    string log = $"[{time}]{Global.Separator}[{ip}]{Global.Separator}[{sessionID}]{Global.Separator}RESPONSE {message}";
                    OnShowLog(log);
                    log = string.Format("({0})_RESPONSE_{1}", time, message);
                    FileHelper.WriteLog("", log, string.Format(FolderName, ip), sessionID);
                }
            }
            
            OnReceived(session, e);
        }

        protected override void OnSessionClosed(ClientSession session, CloseReason reason)
        {
            if (IsShowLog)
            {
                string time = DateTime.Now.ToString(Global.HHmmss);
                string ip = session.RemoteEndPoint.Address.ToString();
                string sessionID = session.SessionID;

                //string log = string.Format("IP({0}_{1}_{2})_SessionClosed_{3}", session.RemoteEndPoint.Address.ToString(), session.SessionID, time, reason);
                //string log = Global.FormatLog.Frmat(time, ip, sessionID, "SessionClosed " + reason); //string.Format("[{0}] [{1}] [{2}] SessionClosed {3}", time, ip, sessionID, reason);
                string log = $"[{time}]{Global.Separator}[{ip}]{Global.Separator}[{sessionID}]{Global.Separator}SessionClosed {reason}{Global.Separator}" +
                             $"(Thời gian kết nối gần nhất {session.LastActiveTime.ToString(Global.HHmmss)})";
                OnShowLog(log);
                log = string.Format("({0})_SessionClosed_{1}", time, reason);
                FileHelper.WriteLog("", log, string.Format(FolderName, ip), sessionID);
            }
            base.OnSessionClosed(session, reason);
        }

        public void SendMsg(ClientSession session, byte[] data)
        {
            if (IsShowLog)
            {
                string time = DateTime.Now.ToString(Global.HHmmss);
                string ip = session.RemoteEndPoint.Address.ToString();
                string sessionID = session.SessionID;
                string message = BitConverter.ToString(data);

                //string log = string.Format("IP({0}_{1}_{2})_REQUEST_{3}", session.RemoteEndPoint.Address.ToString(), session.SessionID, time, BitConverter.ToString(data));
                //string log = Global.FormatLog.Frmat(time, ip, sessionID, "REQUEST " + message); //string.Format("[{0}] [{1}] [{2}] REQUEST {3}", time, ip, sessionID, message);
                string log = $"[{time}]{Global.Separator}[{ip}]{Global.Separator}[{sessionID}]{Global.Separator}REQUEST {message}";
                OnShowLog(log);
                log = string.Format("({0})_REQUEST_{1}", time, message);
                FileHelper.WriteLog("", log, string.Format(FolderName, ip), sessionID);
            }
            session.SendMsg(data);
        }

        //public void SendMsgAll(List<ClientSession> sessions, byte[] data)
        //{
        //    Parallel.ForEach(sessions/*.Where(x => x.IsSelected)*/, session =>
        //    {
        //        SendMsg(session, data);
        //    });
        //}

        public void SendMsgAll(byte[] data)
        {
            if (ClientSessions != null)
            {
                Parallel.ForEach(ClientSessions, session =>
                {
                    SendMsg(session, data);
                });
            }    
        }

        private void OnTimeout(object obj)
        {
            //Nếu quá 3 phiên client không gửi tín hiệu lên server thì coi như client sẽ offline (mỗi phiên là 4s)
            if (obj != null)
            {
                ClientSession session = (ClientSession)obj;

                double timeOff = (DateTime.Now - session.LastActiveTime).TotalSeconds;
                //Nếu lớn hơn Timeout giây thì thiết bị sẽ close session
                if (timeOff > Timeout)
                {
                    string ip = session.RemoteEndPoint?.Address?.ToString();
                    if (!string.IsNullOrEmpty(ip))
                    {
                        session.Close(CloseReason.TimeOut);
                    }
                }
            }
        }
    }
}
