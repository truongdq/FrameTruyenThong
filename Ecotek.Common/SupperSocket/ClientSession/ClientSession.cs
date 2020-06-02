using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ecotek.Common.SupperSocket
{
	public class ClientSession : AppSession<ClientSession, BinaryRequestInfo>
    {
		#region Connect Event
		/// <summary>
		/// Message to be displayed in the UI
		/// </summary>
		public event dgMessage OnMessaged;
		#endregion

		/// <summary>
		/// The identity of the user
		/// </summary>
		public string UserID { get; set; }

		public bool IsSelected { get; set; }

		protected override void OnSessionStarted()
		{
			base.OnSessionStarted();
		}

		protected override void HandleException(Exception e)
		{
			Send("Error : {0}", e.Message);
		}

		protected override void OnSessionClosed(CloseReason reason)
		{
			base.OnSessionClosed(reason);
		}

		/// <summary>
		/// Used to pass a message to the server object.
		/// </summary>
		private void SendMsg_ServerLog(string sMsg)
		{
			LocalMessageEventArgs e = new LocalMessageEventArgs(sMsg, TypeLocal.None);

			OnMessaged(this, e);
		}

		public void SendMsg_User(Command typeCommand, string sData)
		{
			//Create data to send
			DataSend insSend = new DataSend();
			insSend.Command = typeCommand;
			insSend.Data_String = sData;

			//Convert to byte
			byte[] byteSend = insSend.CreateDataOriginal().Data;
			//Send Data
			Send(byteSend, 0, byteSend.Length);
		}

		public void SendMsg_User(Command typeCommand, params string[] sData)
		{
			//Create data to send
			DataSend insSend = new DataSend(typeCommand, sData);
			//Convert to byte
			byte[] byteSend = insSend.CreateDataOriginal().Data;
			//Send
			Send(byteSend, 0, byteSend.Length);
		}

		public async void SendMsgAsync(byte[] data)
		{
			await Task.Run(() =>
			{
				SendMsg(data);
			});
		}

		public void SendMsg(params byte[] sData)
		{
			//Create data to send
			DataSend insSend = new DataSend(sData);
			//Convert to byte
			byte[] byteSend = insSend.CreateDataOriginal().Data;
			//Send Data
			Send(byteSend, 0, byteSend.Length);
		}
	}
}
