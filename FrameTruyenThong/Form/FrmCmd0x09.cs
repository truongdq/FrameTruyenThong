using Ecotek.Business;
using Ecotek.Common;
using Ecotek.Common.Extentions;
using Ecotek.Common.SupperSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameTruyenThong
{
    public partial class FrmCmd0x09 : Form
    {
        private readonly IEnumerable<KeyValuePair<ClientSession, Frame>> _dicSrc0x09;
        private Dictionary<ClientSession, Data0x09> _dicDes0x09;
        private readonly Server _server;

        public FrmCmd0x09(IEnumerable<KeyValuePair<ClientSession, Frame>> dic0x09, Server server)
        {
            InitializeComponent();

            _server = server;
            _dicSrc0x09 = dic0x09;
            _dicDes0x09 = new Dictionary<ClientSession, Data0x09>();
        }

        private void FrmCmd0x09_Load(object sender, EventArgs e)
        {
            LoadDataToGrid();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendData();
            if (MessageBox.Show("Gửi thành công!!!") == DialogResult.OK)
            {
                this.Close();
            }
        }

        public string UnsafeAsciiBytesToString(byte[] buffer, int offset)
        {
            int end = offset;
            while (end < buffer.Length && buffer[end] != 0)
            {
                end++;
            }
            unsafe
            {
                fixed (byte* pAscii = buffer)
                {
                    return new String((sbyte*)pAscii, offset, end - offset);
                }
            }
        }

        private void LoadDataToGrid()
        {
            dtgCmd0x09.Rows.Clear();
            _dicDes0x09 = GetData0x09();

            foreach (var item in _dicDes0x09)
            {
                string sessionID = item.Key.SessionID;
                string ip = item.Key.RemoteEndPoint.Address.ToString();
                string ipECR = ConvertToIPAddress(item.Value.IP_ECR);
                string ipServer = ConvertToIPAddress(item.Value.IP_Server);
                string portServer = (item.Value.Port_Server[0].ConvertToHex() + item.Value.Port_Server[1].ConvertToHex()).ConvertToPort();
                string ssid = UnsafeAsciiBytesToString(item.Value.SSID, 0);
                string ssidLength = item.Value.SSID_Length.ToString();
                string key = UnsafeAsciiBytesToString(item.Value.Key, 0);
                string keyLength = item.Value.Key_Length.ToString();
                string modeECR = item.Value.Mode_ECR.ToString();
                string ipServerECR = ConvertToIPAddress(item.Value.IP_Server_ECR);
                string portServerECR = (item.Value.Port_Server_ECR[0].ConvertToHex() + item.Value.Port_Server_ECR[1].ConvertToHex()).ConvertToPort();

                dtgCmd0x09.Rows.Add(sessionID, ip, ipECR, ipServer, portServer, ssid, ssidLength, key, keyLength, modeECR, ipServerECR, portServerECR);
            }
        }

        private string ConvertToIPAddress(byte[] arr)
        {
            string ip = string.Empty;
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (i != arr.Length - 1)
                    {
                        ip += string.Format("{0}.", arr[i]);
                    }    
                    else
                    {
                        ip += string.Format("{0}", arr[i]);
                    }    
                }    
            }    

            return ip;
        }

        private Dictionary<ClientSession, Data0x09> GetData0x09()
        {
            Dictionary<ClientSession, Data0x09> data = new Dictionary<ClientSession, Data0x09>();

            foreach (var item in _dicSrc0x09)
            {
                ClientSession session = item.Key;
                Data0x09 data0X09 = ConvertByteArrayToData0x09(item.Value.Data);
                if (!data.ContainsKey(session))
                {
                    data.Add(session, data0X09);
                }
                else
                {
                    data[session] = data0X09;
                }
            }

            return data;
        }

        private Data0x09 ConvertByteArrayToData0x09(byte[] arr)
        {
            Data0x09 data0X09 = new Data0x09();

            try
            {
                int indexOfArr = 0;

                //4 byte IP ECR
                for (int i = 0; i < data0X09.IP_ECR.Length; i++)
                {
                    data0X09.IP_ECR[i] = arr[indexOfArr];
                    indexOfArr++;
                }

                //4 Byte IP SERVER
                for (int i = 0; i < data0X09.IP_Server.Length; i++)
                {
                    data0X09.IP_Server[i] = arr[indexOfArr];
                    indexOfArr++;
                }

                //2 Byte Port SERVER
                for (int i = 0; i < data0X09.Port_Server.Length; i++)
                {
                    data0X09.Port_Server[i] = arr[indexOfArr];
                    indexOfArr++;
                }

                //1 Byte độ dài SSID
                data0X09.SSID_Length = arr[indexOfArr];
                indexOfArr++;

                //32 Byte SSID
                for (int i = 0; i < data0X09.SSID.Length; i++)
                {
                    data0X09.SSID[i] = arr[indexOfArr];
                    indexOfArr++;
                }

                //1 Byte độ dài KEY
                data0X09.Key_Length = arr[indexOfArr];
                indexOfArr++;

                //16 byte KEY
                for (int i = 0; i < data0X09.Key.Length; i++)
                {
                    data0X09.Key[i] = arr[indexOfArr];
                    indexOfArr++;
                }

                //1 Byte chế độ ECR (ECR = 0, ECR_RF = 1)
                data0X09.Mode_ECR = arr[indexOfArr];
                indexOfArr++;

                //4 Byte IP SERVER của ECR kết nối với tủ trung tâm trong chế độ tập tự do không chạy phần mềm trên máy tính
                for (int i = 0; i < data0X09.IP_Server_ECR.Length; i++)
                {
                    data0X09.IP_Server_ECR[i] = arr[indexOfArr];
                    indexOfArr++;
                }

                //2 Byte Port SERVER của ECR kết nối với tủ trung tâm trong chế độ tập tự do không chạy phần mềm trên máy tính
                for (int i = 0; i < data0X09.Port_Server_ECR.Length; i++)
                {
                    data0X09.Port_Server_ECR[i] = arr[indexOfArr];
                    indexOfArr++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình phân tích dữ liệu", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return data0X09;
        }

        private void SendData()
        {
            ConcurrentDictionary<ClientSession, Frame> data = new ConcurrentDictionary<ClientSession, Frame>();

            foreach (var item in _dicDes0x09)
            {
                ClientSession session = item.Key;
                string sessionID = session.SessionID;
                string keyNode = string.Empty;

                Frame frame = new Frame()
                {
                    Code = Constants.Hx09,
                    Data = new byte[item.Value.Length]
                };

                int indexOffArr = 0;

                //4 byte IP ECR
                for (int i = 0; i < item.Value.IP_ECR.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.IP_ECR[i];
                    indexOffArr++;
                }

                //4 Byte IP SERVER
                for (int i = 0; i < item.Value.IP_Server.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.IP_Server[i];
                    indexOffArr++;
                }

                //2 Byte Port SERVER
                for (int i = 0; i < item.Value.Port_Server.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.Port_Server[i];
                    indexOffArr++;
                }

                //1 Byte độ dài SSID
                frame.Data[indexOffArr] = item.Value.SSID_Length;
                indexOffArr++;

                //32 Byte SSID
                for (int i = 0; i < item.Value.SSID.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.SSID[i];
                    indexOffArr++;
                }

                //1 Byte độ dài KEY
                frame.Data[indexOffArr] = item.Value.Key_Length;
                indexOffArr++;

                //16 byte KEY
                for (int i = 0; i < item.Value.Key.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.Key[i];
                    indexOffArr++;
                }

                //1 Byte chế độ ECR (ECR = 0, ECR_RF = 1)
                frame.Data[indexOffArr] = item.Value.Mode_ECR;
                indexOffArr++;

                //4 Byte IP SERVER của ECR kết nối với tủ trung tâm trong chế độ tập tự do không chạy phần mềm trên máy tính
                for (int i = 0; i < item.Value.IP_Server_ECR.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.IP_Server_ECR[i];
                    indexOffArr++;
                }

                //2 Byte Port SERVER của ECR kết nối với tủ trung tâm trong chế độ tập tự do không chạy phần mềm trên máy tính
                for (int i = 0; i < item.Value.Port_Server_ECR.Length; i++)
                {
                    frame.Data[indexOffArr] = item.Value.Port_Server_ECR[i];
                    indexOffArr++;
                }

                if (!data.ContainsKey(session))
                {
                    data.TryAdd(session, frame);
                }
                else
                {
                    data[session] = frame;
                }
            }

            Parallel.ForEach(data, x =>
            {
                _server.SendMsg(x.Key, x.Value.Get());
            });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //LoadNodesParent();
        }

        private void dtgCmd0x09_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!ValidateData(e.RowIndex))
            {
                return;
            }

            int rowIndex = e.RowIndex;
            string sessionID = dtgCmd0x09.Rows[rowIndex].Cells["colSessionID"].Value?.ToString();
            var session = _dicDes0x09.First(x => x.Key.SessionID == sessionID).Key;

            //IP của ECR
            var ipECR = dtgCmd0x09.Rows[rowIndex].Cells["colIPECR"].Value?.ToString();
            _dicDes0x09[session].IP_ECR = GetDataIPAddress(ipECR);

            //IP của Server
            var ipServer = dtgCmd0x09.Rows[rowIndex].Cells["colIPServer"].Value?.ToString();
            _dicDes0x09[session].IP_ECR = GetDataIPAddress(ipServer);

            //Port của Server
            int portServer = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells["colPortServer"].Value);
            _dicDes0x09[session].Port_Server[0] = (byte)portServer.LengthHigh();
            _dicDes0x09[session].Port_Server[1] = (byte)portServer.LengthLow();

            //Độ dài SSID
            int ssidLength = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells["colSSIDLength"].Value);
            _dicDes0x09[session].SSID_Length = (byte)ssidLength;

            //SSID
            var ssid = dtgCmd0x09.Rows[rowIndex].Cells["colSSID"].Value?.ToString();
            var ssidByteArr = ssid.ToByteArray();
            for (int i = 0; i < _dicDes0x09[session].SSID.Length; i++)
            {
                if (i < ssidByteArr.Length)
                {
                    _dicDes0x09[session].SSID[i] = ssidByteArr[i];
                }    
                else
                {
                    _dicDes0x09[session].SSID[i] = Constants.Hx00;
                }    
            }


            //Độ dài Key
            int keyLength = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells["colKeyLength"].Value);
            _dicDes0x09[session].Key_Length = (byte)keyLength;

            //Key
            var key = dtgCmd0x09.Rows[rowIndex].Cells["colKey"].Value?.ToString();
            var keyByteArr = key.ToByteArray();
            for (int i = 0; i < _dicDes0x09[session].Key.Length; i++)
            {
                if (i < keyByteArr.Length)
                {
                    _dicDes0x09[session].Key[i] = keyByteArr[i];
                }
                else
                {
                    _dicDes0x09[session].Key[i] = Constants.Hx00;
                }
            }

            //Chế độ ECR
            byte modeECR = Convert.ToByte(dtgCmd0x09.Rows[rowIndex].Cells["colModeECR"].Value);
            _dicDes0x09[session].Mode_ECR = modeECR;

            //IP Server của ECR
            var ipServerECR = dtgCmd0x09.Rows[rowIndex].Cells["colIPServerECR"].Value?.ToString();
            _dicDes0x09[session].IP_Server_ECR = GetDataIPAddress(ipServerECR);

            //Port Server của ECR
            int portServerECR = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells["colPortServerECR"].Value);
            _dicDes0x09[session].Port_Server_ECR[0] = (byte)portServerECR.LengthHigh();
            _dicDes0x09[session].Port_Server_ECR[1] = (byte)portServerECR.LengthLow();
        }

        private bool ValidateData(int rowIndex)
        {
            bool isValid = true;

            //IP của ECR
            var ipECR = dtgCmd0x09.Rows[rowIndex].Cells["colIPECR"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colIPECR"].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(ipECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells["colIPECR"].ErrorText = "IP không được trống";
                isValid = false;
            }
            else
            {
                if (!ValidIPAddress(ipECR))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colIPECR"].ErrorText = "IP không hợp lệ";
                    isValid = false;
                }
            }

            //IP của Server
            var ipServer = dtgCmd0x09.Rows[rowIndex].Cells["colIPServer"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colIPServer"].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(ipServer))
            {
                dtgCmd0x09.Rows[rowIndex].Cells["colIPServer"].ErrorText = "IP không được trống";
                isValid = false;
            }
            else
            {
                if (!ValidIPAddress(ipServer))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colIPServer"].ErrorText = "IP không hợp lệ";
                    isValid = false;
                }
            }

            //Port của Server
            var portServer = dtgCmd0x09.Rows[rowIndex].Cells["colPortServer"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colPortServer"].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(portServer))
            {
                dtgCmd0x09.Rows[rowIndex].Cells["colPortServer"].ErrorText = "Port không được trống";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(portServer, out int result))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colPortServer"].Value = int.MaxValue;
                }    
            }    


            //SSID
            var ssid = dtgCmd0x09.Rows[rowIndex].Cells["colSSID"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colSSID"].ErrorText = string.Empty;
            if (!string.IsNullOrEmpty(ssid))
            {
                if (ssid.Length > 32)
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colSSID"].ErrorText = "Độ dài vượt quá 32 ký tự";
                    isValid = false;
                }

                dtgCmd0x09.Rows[rowIndex].Cells["colSSIDLength"].Value = ssid.Length;
            }

            //KEY
            var key = dtgCmd0x09.Rows[rowIndex].Cells["colKey"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colKey"].ErrorText = string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                if (key.Length > 16)
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colKey"].ErrorText = "Độ dài vượt quá 16 ký tự";
                    isValid = false;
                }

                dtgCmd0x09.Rows[rowIndex].Cells["colKeyLength"].Value = key.Length;
            }

            //Chế độ ECR
            var modeECR = dtgCmd0x09.Rows[rowIndex].Cells["colModeECR"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colModeECR"].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(modeECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells["colModeECR"].ErrorText = "Không được trống";
                isValid = false;
            }  
            else
            {
                if (!modeECR.Equals("0") && !modeECR.Equals("1"))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colModeECR"].Value = "0";
                }    
            }    

            //IP Server của ECR
            var ipServerECR = dtgCmd0x09.Rows[rowIndex].Cells["colIPServerECR"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colIPServerECR"].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(ipServerECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells["colIPServerECR"].ErrorText = "IP không được trống";
                isValid = false;
            }
            else
            {
                if (!ValidIPAddress(ipServerECR))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colIPServerECR"].ErrorText = "IP không hợp lệ";
                    isValid = false;
                }
            }

            //Port Server của ECR
            var portServerECR = dtgCmd0x09.Rows[rowIndex].Cells["colPortServerECR"].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells["colPortServerECR"].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(portServerECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells["colPortServerECR"].ErrorText = "Port không được trống";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(portServerECR, out int result))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells["colPortServerECR"].Value = int.MaxValue;
                }
            }

            return isValid;
        }

        private bool ValidIPAddress(string ipAddress)
        {
            IPAddress ip;
            return IPAddress.TryParse(ipAddress, out ip);
        }

        private byte[] GetDataIPAddress(string ipAddress)
        {
            var arr = ipAddress.Split('.');
            byte[] ipArr = new byte[arr.Length];
            for(int i = 0; i < arr.Length; i++)
            {
                ipArr[i] = Convert.ToByte(arr[i]);
            }

            return ipArr;
        }

        private void dtgCmd0x09_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var currentCell = dtgCmd0x09.CurrentCell;

            if (currentCell.ColumnIndex == 4 || currentCell.ColumnIndex == 11)
            {
                ((TextBox)e.Control).KeyPress += OnCellPortKeyPress;
            }
            else if (currentCell.ColumnIndex == 2 || currentCell.ColumnIndex == 3 || currentCell.ColumnIndex == 10)
            {
                ((TextBox)e.Control).KeyPress += OnCellIPAddressKeyPress;
            }   
            else if (currentCell.ColumnIndex == 9)
            {
                ((TextBox)e.Control).KeyPress += OnCellModeECRKeyPress;
            }    
        }

        private void OnCellPortKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void OnCellIPAddressKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void OnCellModeECRKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete && e.KeyChar != '0' && e.KeyChar != '1')
            {
                e.Handled = true;
            }
        }

        private void dtgCmd0x09_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
               
        }
    }

    
}
