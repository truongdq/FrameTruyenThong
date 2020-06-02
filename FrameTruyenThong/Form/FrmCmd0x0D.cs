using Ecotek.Business;
using Ecotek.Common;
using Ecotek.Common.Extentions;
using Ecotek.Common.SupperSocket;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace FrameTruyenThong
{
    public partial class FrmCmd0x0D : Form
    {
        List<ClientSession> _sessions;
        Server _server;

        public FrmCmd0x0D(List<ClientSession> sessions, Server server)
        {
            InitializeComponent();

            _sessions = sessions;
            _server = server;

            txtIPAddress.Text = GetLocalIPAddress();
            txtPort.Text = server.Config.Port.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                if (_server != null && _sessions != null && _sessions.Count > 0)
                {
                    var arr = txtIPAddress.Text.Trim().Split('.');
                    int port = Convert.ToInt32(txtPort.Text.Trim());

                    Frame frame = new Frame()
                    {
                        Code = Constants.Hx0D,
                        Data = new byte[]
                        {
                    Convert.ToByte(arr[0]),
                    Convert.ToByte(arr[1]),
                    Convert.ToByte(arr[2]),
                    Convert.ToByte(arr[3]),
                    (byte)port.LengthLow(),
                    (byte)port.LengthHigh(),
                        }
                    };

                    //_server.SendMsgAll(_sessions, frame.Get());
                    _server.SendMsgAll(frame.Get());
                    if (MessageBox.Show("Gửi thành công!!!") == DialogResult.OK)
                    {
                        this.Close();
                    }    
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi thất bại!!!");
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void txtIPAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private bool ValidateInput()
        {
            bool flag = true;

            if (string.IsNullOrEmpty(txtIPAddress.Text.Trim()))
            {
                MessageBox.Show("Địa chỉ IP không được trống");
                flag = false;
            }    

            if (string.IsNullOrEmpty(txtPort.Text.Trim()))
            {
                MessageBox.Show("Port không được trống");
                flag = false;
            }

            var arr = txtIPAddress.Text.Trim().Split('.');
            if (arr.Length != 4)
            {
                MessageBox.Show("Địa chỉ IP không đúng định dạng");
                flag = false;
            }

            return flag;
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return string.Empty;
        }
    }
}
