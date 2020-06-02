using System;

namespace Ecotek.Common.SupperSocket
{
    /// <summary>
    /// Format for message events.
    /// </summary>
    public class LocalMessageEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        ///Message
        /// </summary>
        public string Message { set; get; } = "";

        public byte[] bMessage { set; get; }

        /// <summary>
        /// Message type
        /// </summary>
        public TypeLocal Icon { set; get; } = TypeLocal.None;
        #endregion

        /// <summary>
        /// Message settings
        /// </summary>
        public LocalMessageEventArgs(string strMsg, TypeLocal typeIcon)
        {
            Message = strMsg;
            Icon = typeIcon;
        }

        public LocalMessageEventArgs(byte[] body, TypeLocal typeIcon)
        {
            bMessage = body;
            Icon = typeIcon;
        }

        public LocalMessageEventArgs()
        {
            Message = "";
            Icon = TypeLocal.None;
        }
    }
}
