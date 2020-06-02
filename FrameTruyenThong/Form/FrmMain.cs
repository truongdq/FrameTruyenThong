using Ecotek.Business;
using Ecotek.Business.Business.Command;
using Ecotek.Common;
using Ecotek.Common.Extentions;
using Ecotek.Common.SupperSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FrameTruyenThong
{
    public partial class FrmMain : FrmBase
    {
        private const string COLUMN_NAME_SESSIONID = "colSessionID";
        private const string COLUMN_NAME_SESSIONID_2 = "colSessionID2";
        private const string COLUMN_NAME_IP_ECR = "colIPECR";
        private const string COLUMN_NAME_IP_SERVER = "colIPServer";
        private const string COLUMN_NAME_PORT_SERVER = "colPortServer";
        private const string COLUMN_NAME_SSID_LENGTH = "colSSIDLength";
        private const string COLUMN_NAME_SSID = "colSSID";
        private const string COLUMN_NAME_KEY_LENGTH = "colKeyLength";
        private const string COLUMN_NAME_KEY = "colKey";
        private const string COLUMN_NAME_MODE_ECR = "colModeECR";
        private const string COLUMN_NAME_IP_SERVER_ECR = "colIPServerECR";
        private const string COLUMN_NAME_PORT_SERVER_ECR = "colPortServerECR";

        private const int LENGTH_FRAME = 256;

        private int CLEAR_LOG_COUNT = Convert.ToInt32(ConfigurationManager.AppSettings["CLEAR_LOG_COUNT"]);
        private int _count = 0;
        private Server _server;
        private ConcurrentDictionary<ClientSession, Data0x01> _dic0x01 = new ConcurrentDictionary<ClientSession, Data0x01>();
        private List<Frame> _frameList = new List<Frame>();
        private ConcurrentDictionary<ClientSession, Data0x09> _dic0x09 = new ConcurrentDictionary<ClientSession, Data0x09>();
        private ProcessorCmd0x0A _cmd0x0A;

        public FrmMain()
        {
            InitializeComponent();
            EnableControls(false);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            dtgXe.LoadCheckBox();
            dtgXe.OnHeaderCheckBoxClick += dtgXe_OnHeaderCheckBoxClick;
            dtgXe.OnCellCheckBoxClick += dtgXe_OnCellCheckBoxClick;

            dtgCmd0x09.LoadCheckBox();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnStart.Text == "Start")
                {
                    if (_server == null)
                    {
                        _server = new Server() { IsShowLog = chkShowLog.Checked, Timeout = 12 };
                        _server.NewSessionConnected += Server_NewSessionConnected;
                        _server.SessionClosed += Server_SessionClosed;
                        _server.OnReceived += Server_OnReceived;
                        _server.OnShowLog += Server_OnShowLog;

                        if (!SuperSocketSetup(_server)) return;
                    }

                    if (!_server.Start()) return;
                    ShowLog("Server đã bật");
                    tsslStartTime.Text = DateTime.Now.ToString(Global.ddMMyyyyHHmmss);
                    btnStart.Text = "Stop";
                    EnableControls(true);
                }
                else
                {
                    _server.Stop();
                    btnStart.Text = "Start";
                    EnableControls(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động server. Vui lòng liên hệ quản trị viên để được hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void btn0x08_Click(object sender, EventArgs e)
        {
            if (!CheckRowSelect(dtgXe))
            {
                tbCtrlInfo.SelectedTab = tbPageInfoCar;
                return;
            }

            tbCtrlInfo.SelectedTab = tbPageCmd0x08;

            List<ClientSession> clientSessions = new List<ClientSession>();
            foreach (var session in _server.ClientSessions)
            {
                if (IsRowSelected(session.SessionID, dtgXe, COLUMN_NAME_SESSIONID))
                {
                    clientSessions.Add(session);
                }
            }

            ProcessorCmd0x08 cmd0x08 = new ProcessorCmd0x08() { Server = _server };
            cmd0x08.SendMsgAll(clientSessions);
        }

        private void btn0x09_Click(object sender, EventArgs e)
        {
            if (!CheckRowSelect(dtgCmd0x09)) return;

            ProcessorCmd0x09 cmd0x09 = new ProcessorCmd0x09()
            {
                Server = _server,
                Data = _dic0x09.Where(x => IsRowSelected(x.Key.SessionID, dtgCmd0x09, COLUMN_NAME_SESSIONID_2)).ToList()
            };
            cmd0x09.SendMsg();
        }

        private void btn0x0A_Click(object sender, EventArgs e)
        {
            if (!CheckRowSelect(dtgXe)) return;

            if (!File.Exists(txtSelectFile.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn file!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (_cmd0x0A == null)
            {
                _cmd0x0A = new ProcessorCmd0x0A()
                {
                    Server = _server,
                    Frames = _frameList,
                };
            }
            else
            {
                _cmd0x0A.Frames = _frameList;
            }

            _cmd0x0A.OnUpdateThongSo += cmd0x0A_OnUpdateThongSo;

            _cmd0x0A.SendMsgAll(_server.ClientSessions.Where(x => IsRowSelected(x.SessionID, dtgXe, COLUMN_NAME_SESSIONID)).ToList());
        }

        private void btn0x0D_Click(object sender, EventArgs e)
        {
            if (!CheckRowSelect(dtgXe)) return;

            List<ClientSession> clientSessions = new List<ClientSession>();
            foreach (var session in _server.ClientSessions.Where(x => IsRowSelected(x.SessionID, dtgXe, COLUMN_NAME_SESSIONID)))
            {
                clientSessions.Add(session);
            }

            FrmCmd0x0D frmCmd0X0D = new FrmCmd0x0D(clientSessions, _server);
            frmCmd0X0D.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        private void Server_OnShowLog(string content)
        {
            ShowLog(content);
        }

        private void Server_NewSessionConnected(ClientSession session)
        {
            Exec(() =>
            {
                string sessionID = session.SessionID;

                if (_server.ClientSessions.FirstOrDefault(x => x.SessionID == session.SessionID) == null)
                {
                    dtgXe.Rows.Add(false, sessionID, "", "", "");
                }

                _server.ClientSessions.Add(session);

                if (!_dic0x01.ContainsKey(session))
                {
                    _dic0x01.TryAdd(session, new Data0x01() { SessionID = sessionID });
                }
                else
                {
                    _dic0x01[session].SessionID = sessionID;
                }
            });
        }

        private void Server_SessionClosed(ClientSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Exec(() =>
            {
                _server.ClientSessions.Remove(session);

                string sessionID = session.SessionID;

                if (_dic0x01.ContainsKey(session))
                {
                    _dic0x01.TryRemove(session, out Data0x01 xe);
                }

                if (_dic0x09.ContainsKey(session))
                {
                    _dic0x09.TryRemove(session, out Data0x09 data0x09);
                }

                RemoveRowGridView(sessionID);

                dtgXe.CheckedHeaderGridView();
            });
        }

        private void Server_OnReceived(ClientSession session, LocalMessageEventArgs e)
        {
            try
            {
                if (e.bMessage[0] == Constants.HxAA && e.bMessage[1] == Constants.Hx55 && e.bMessage[2] == Constants.HxCC)
                {

                    SendData_Analysis insSD_A = new SendData_Analysis(e.bMessage);
                    string response = BitConverter.ToString(e.bMessage);
                    byte codeCmd = e.bMessage[3];
                    switch (codeCmd)
                    {
                        case Constants.Hx00:
                            Connect(session, insSD_A);
                            break;
                        case Constants.Hx01:
                            Ping(session, insSD_A);
                            //Cập nhật trạng thái 3 byte cuối của DATA
                            if (_dic0x01.ContainsKey(session))
                            {
                                int indexEnd = e.bMessage.Length - 1;
                                _dic0x01[session].PingPong_2 = e.bMessage[indexEnd - 1].ToString();
                                _dic0x01[session].PingPong_1 = e.bMessage[indexEnd - 2].ToString();
                                _dic0x01[session].PingPong_0 = e.bMessage[indexEnd - 3].ToString();
                            }
                            break;
                        case Constants.Hx08:
                            StoreData0x09(session, e.bMessage);
                            break;
                        case Constants.Hx09:
                            break;
                        case Constants.Hx0A:
                            _cmd0x0A.Process0x0A(session, false);
                            break;
                        case Constants.Hx0B:
                            UpdateThongSoXe(session, "Thành công");
                            break;
                        case Constants.Hx0C:
                            UpdateThongSoXe(session, "Không thành công");
                            break;
                        case Constants.Hx0D:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void cmd0x0A_OnUpdateThongSo(string sessionID, int frameSendCount, int frameReceiveCount, int frameTotal, string state)
        {
            Exec(() =>
            {
                var row = dtgXe.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()) &&
                                                                                 sessionID.Equals(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()));
                if (row != null)
                {
                    row.Cells["colFrameSendCount"].Value = frameSendCount;
                    row.Cells["colFrameReceiveCount"].Value = frameReceiveCount;
                    row.Cells["colFrameTotal"].Value = frameTotal;
                    row.Cells["colState"].Value = state;
                }

                dtgXe.Refresh();
            });
        }

        private void dtgXe_OnCellCheckBoxClick(int rowIndex, bool isChecked)
        {
            string sessionID = dtgXe.Rows[rowIndex].Cells[COLUMN_NAME_SESSIONID].Value?.ToString();

            if (!string.IsNullOrEmpty(sessionID))
            {
                var clientSession = _server.ClientSessions.FirstOrDefault(x => x.SessionID.Equals(sessionID));

                if (clientSession != null)
                {
                    clientSession.IsSelected = isChecked;

                    UpdatePingPongXe(clientSession);
                }
            }
        }

        private void dtgXe_OnHeaderCheckBoxClick(bool isChecked)
        {
            foreach (DataGridViewRow row in dtgXe.Rows)
            {
                string sessionID = row.Cells[COLUMN_NAME_SESSIONID].Value?.ToString();
                if (!string.IsNullOrEmpty(sessionID))
                {
                    var clientSession = _server.ClientSessions.FirstOrDefault(x => x.SessionID.Equals(sessionID));

                    if (clientSession != null)
                    {
                        clientSession.IsSelected = isChecked;
                        UpdatePingPongXe(clientSession);
                    }
                }
            }
        }

        private void dtgCmd0x09_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!ValidateData0x09(e.RowIndex))
                {
                    return;
                }

                int rowIndex = e.RowIndex;
                string sessionID = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SESSIONID_2].Value?.ToString();
                var session = _dic0x09.First(x => x.Key.SessionID == sessionID).Key;

                //IP của ECR
                var ipECR = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_ECR].Value?.ToString();
                _dic0x09[session].IP_ECR = GetDataIPAddress(ipECR);

                //IP của Server
                var ipServer = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER].Value?.ToString();
                _dic0x09[session].IP_Server = GetDataIPAddress(ipServer);

                //Port của Server
                int portServer = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER].Value);
                _dic0x09[session].Port_Server[0] = (byte)portServer.LengthHigh();
                _dic0x09[session].Port_Server[1] = (byte)portServer.LengthLow();

                //Độ dài SSID
                int ssidLength = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID_LENGTH].Value);
                _dic0x09[session].SSID_Length = (byte)ssidLength;

                //SSID
                var ssid = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID].Value?.ToString();
                var ssidByteArr = ssid?.ToByteArray();
                for (int i = 0; i < _dic0x09[session].SSID.Length; i++)
                {
                    if (i < ssidByteArr?.Length)
                    {
                        _dic0x09[session].SSID[i] = ssidByteArr[i];
                    }
                    else
                    {
                        _dic0x09[session].SSID[i] = Constants.Hx00;
                    }
                }


                //Độ dài Key
                int keyLength = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY_LENGTH].Value);
                _dic0x09[session].Key_Length = (byte)keyLength;

                //Key
                var key = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY].Value?.ToString();
                var keyByteArr = key?.ToByteArray();
                for (int i = 0; i < _dic0x09[session].Key.Length; i++)
                {
                    if (i < keyByteArr?.Length)
                    {
                        _dic0x09[session].Key[i] = keyByteArr[i];
                    }
                    else
                    {
                        _dic0x09[session].Key[i] = Constants.Hx00;
                    }
                }

                //Chế độ ECR
                byte modeECR = Convert.ToByte(dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_MODE_ECR].Value);
                _dic0x09[session].Mode_ECR = modeECR;

                //IP Server của ECR
                var ipServerECR = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].Value?.ToString();
                _dic0x09[session].IP_Server_ECR = GetDataIPAddress(ipServerECR);

                //Port Server của ECR
                int portServerECR = Convert.ToInt32(dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER_ECR].Value);
                _dic0x09[session].Port_Server_ECR[0] = (byte)portServerECR.LengthHigh();
                _dic0x09[session].Port_Server_ECR[1] = (byte)portServerECR.LengthLow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra trong quá trình nhập liệu thông tin thiết bị", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void dtgCmd0x09_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var currentCell = dtgCmd0x09.CurrentCell;

            int indexPortServer = dtgCmd0x09.Rows[currentCell.RowIndex].Cells[COLUMN_NAME_PORT_SERVER].ColumnIndex;
            int indexPortServerECR = dtgCmd0x09.Rows[currentCell.RowIndex].Cells[COLUMN_NAME_PORT_SERVER_ECR].ColumnIndex;
            int indexIPECR = dtgCmd0x09.Rows[currentCell.RowIndex].Cells[COLUMN_NAME_IP_ECR].ColumnIndex;
            int indexIPServer = dtgCmd0x09.Rows[currentCell.RowIndex].Cells[COLUMN_NAME_IP_SERVER].ColumnIndex;
            int indexIPServerECR = dtgCmd0x09.Rows[currentCell.RowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].ColumnIndex;
            int indexModeECR = dtgCmd0x09.Rows[currentCell.RowIndex].Cells[COLUMN_NAME_MODE_ECR].ColumnIndex;

            e.Control.KeyPress -= OnCellPortKeyPress;
            e.Control.KeyPress -= OnCellIPAddressKeyPress;
            e.Control.KeyPress -= OnCellModeECRKeyPress;

            if (currentCell.ColumnIndex == indexPortServer || currentCell.ColumnIndex == indexPortServerECR)
            {
                ((TextBox)e.Control).KeyPress += OnCellPortKeyPress;
            }
            else if (currentCell.ColumnIndex == indexIPECR || currentCell.ColumnIndex == indexIPServer || currentCell.ColumnIndex == indexIPServerECR)
            {
                ((TextBox)e.Control).KeyPress += OnCellIPAddressKeyPress;
            }
            else if (currentCell.ColumnIndex == indexModeECR)
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

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "hex files (*.hex)|*.hex"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSelectFile.Text = openFileDialog.FileName;

                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        IntelHex intelHex = new IntelHex();
                        var data = intelHex.GetDataSegment(reader);
                        List<Frame> frameList = new List<Frame>();

                        if (data != null && data.Length > 0)
                        {
                            int count = data.Length / LENGTH_FRAME;
                            if (data.Length % LENGTH_FRAME != 0)
                            {
                                count++;
                            }

                            for (int i = 0; i < count; i++)
                            {
                                Frame frame = new Frame()
                                {
                                    Code = Constants.Hx0A,
                                    Data = new byte[LENGTH_FRAME]
                                };

                                for (int j = 0; j < LENGTH_FRAME; j++)
                                {
                                    int index = (i * LENGTH_FRAME) + j;
                                    if (index < data.Length)
                                    {
                                        frame.Data[j] = data[index];
                                    }
                                    else
                                    {
                                        frame.Data[j] = Constants.HxFF;
                                    }
                                }

                                frameList.Add(frame);
                            }
                        }

                        _frameList = frameList;
                        if (frameList.Count > 0)
                        {
                            btn0x0A.Enabled = true;
                        }
                        else
                        {
                            btn0x0A.Enabled = false;
                        }

                        //rtbLog.Text = intelHex.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình xử lý file.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void chkShowLog_CheckedChanged(object sender, EventArgs e)
        {
            _server.IsShowLog = chkShowLog.Checked;
        }

        private void ShowLog(string content)
        {
            Exec(() =>
            {
                if (chkShowLog.Checked)
                {
                    if (_count == CLEAR_LOG_COUNT)
                    {
                        rtbLog.Clear();
                        _count = 1;
                    }

                    var arrString = content.Split(new string[] { Global.Separator }, StringSplitOptions.None);
                    if (arrString.Length >= 3)
                    {
                        rtbLog.AppendText(arrString[0] + " ", Color.FromArgb(0, 192, 192));
                        rtbLog.AppendText(arrString[1] + " ", Color.Green);
                        rtbLog.AppendText(arrString[2] + " ", Color.GreenYellow);

                        for (int i = 3; i < arrString.Length; i++)
                        {
                            rtbLog.AppendText(arrString[i] + " ", Color.White);
                        }

                        rtbLog.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        rtbLog.AppendText(content + Environment.NewLine);
                    }
                    rtbLog.ScrollToCaret();
                    _count++;
                }
            });
        }

        public bool SuperSocketSetup(Server server)
        {
            var serverConfig = new SuperSocket.SocketBase.Config.ServerConfig
            {
                Name = "DrivingSuperSocket",//The name of the server instance
                ServerType = "DrivingTest.Socket.BinaryServer, Drivingtest.Socket",
                Ip = "Any",//Any - all IPv4 addresses IPv6Any - all IPv6 addresses
                Mode = SuperSocket.SocketBase.SocketMode.Tcp,//The mode the server is running, Tcp (default) or Udp
                Port = Convert.ToInt32(txtPort.Text.Trim()),//Server listening port
                SendingQueueSize = 10,//The maximum length of the send queue. The default value is 5.
                MaxConnectionNumber = 10000,//Maximum number of connections that can be allowed to connect
                LogCommand = false,//Whether to record the record of command execution
                LogBasicSessionActivity = false,// Whether to record the basic activities of the session, such as connecting and disconnecting
                LogAllSocketException = false,//Whether to record all Socket exceptions and errors
                MaxRequestLength = 1024 * 10,//Maximum allowed request length, default is 1024
                KeepAliveTime = 600,//The interval for sending keep alive data under normal network connection. The default value is 600, in seconds.
                KeepAliveInterval = 60,//After the keep alive fails, keep alive probe packet sending interval, the default value is 60, the unit is second
                //IdleSessionTimeOut = 60,
                //SendTimeOut = 1,
                ClearIdleSession = false, // Whether to empty the idle session periodically, the default value is false;
                ClearIdleSessionInterval = 6//: The interval for clearing idle sessions. The default value is 120, in seconds.
            };

            var rootConfig = new SuperSocket.SocketBase.Config.RootConfig()
            {
                MaxWorkingThreads = 1000,//The maximum number of worker threads in the thread pool
                MinWorkingThreads = 10,// The minimum number of worker threads in the thread pool;
                MaxCompletionPortThreads = 1000,// thread pool maximum completion port thread number;
                MinCompletionPortThreads = 10,// The thread pool minimum completion port thread number;
                DisablePerformanceDataCollector = true,// Whether to disable performance data collection;
                PerformanceDataCollectInterval = 60,// Performance data acquisition frequency (in seconds, default: 60);
                Isolation = SuperSocket.SocketBase.IsolationMode.AppDomain// Server instance isolation level

            };

            return server.Setup(rootConfig, serverConfig);
        }

        private void EnableControls(bool isEnable)
        {
            btnSelectFile.Enabled = isEnable;
            txtSelectFile.Enabled = isEnable;
            btn0x08.Enabled = isEnable;
            btn0x09.Enabled = isEnable;
            btn0x0A.Enabled = isEnable;
            btn0x0D.Enabled = isEnable;
            dtgXe.Enabled = isEnable;
            txtPort.ReadOnly = isEnable;
        }

        private void UpdatePingPongXe(ClientSession session)
        {
            Exec(() =>
            {
                string sessionID = session.SessionID;
                var row = dtgXe.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()) &&
                                                                                 sessionID.Equals(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()));
                if (row != null)
                {
                    if (_dic0x01.ContainsKey(session))
                    {
                        row.Cells["colPingPong_0"].Value = _dic0x01[session].PingPong_0;
                        row.Cells["colPingPong_1"].Value = _dic0x01[session].PingPong_1;
                        row.Cells["colPingPong_2"].Value = _dic0x01[session].PingPong_2;
                    }
                }

                dtgXe.Refresh();
            });
        }

        private void UpdateThongSoXe(ClientSession session, string state)
        {
            Exec(() =>
            {
                string sessionID = session.SessionID;
                var row = dtgXe.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()) &&
                                                                                 sessionID.Equals(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()));
                if (row != null)
                {
                    row.Cells["colState"].Value = state;
                    dtgXe.Refresh();
                }
            });
        }

        private void Connect(ClientSession session, SendData_Analysis insSD_A)
        {
            Exec(() =>
            {
                ArraySegment<byte> pBuffer = new ArraySegment<byte>(insSD_A.DataSend.Data_Byte,
                                                                            0,
                                                                            insSD_A.DataSend.Data_Byte.Length);

                session.Send(new ArraySegment<byte>(pBuffer.Array));
            });
        }

        private void Ping(ClientSession session, SendData_Analysis insSD_A)
        {
            Exec(() =>
            {
                ArraySegment<byte> pBuffer = new ArraySegment<byte>(insSD_A.DataSend.Data_Byte,
                                                                            0,
                                                                            insSD_A.DataSend.Data_Byte.Length);
                session.Send(new ArraySegment<byte>(pBuffer.Array));
            });
        }

        private void StoreData0x09(ClientSession session, byte[] message)
        {
            Exec(() =>
            {
                byte[] data = new byte[message.Length - 7];
                Buffer.BlockCopy(message, 6, data, 0, message.Length - 7);

                Data0x09 data0X09 = ConvertByteArrayToData0x09(data);
                if (!_dic0x09.ContainsKey(session))
                {
                    _dic0x09.TryAdd(session, data0X09);
                    InsertRow0x09(session, data0X09);
                }
                else
                {
                    _dic0x09[session] = data0X09;
                    UpdateRow0x09(session, data0X09);
                }
            });
        }

        private void InsertRow0x09(ClientSession session, Data0x09 data)
        {
            string sessionID = session.SessionID;
            string ipECR = ConvertToIPAddress(data.IP_ECR);
            string ipServer = ConvertToIPAddress(data.IP_Server);
            string portServer = (data.Port_Server[0].ConvertToHex() + data.Port_Server[1].ConvertToHex()).ConvertToPort();
            string ssid = data.SSID.UnsafeAsciiBytesToString(0);
            string ssidLength = data.SSID_Length.ToString();
            string key = data.Key.UnsafeAsciiBytesToString(0);
            string keyLength = data.Key_Length.ToString();
            string modeECR = data.Mode_ECR.ToString();
            string ipServerECR = ConvertToIPAddress(data.IP_Server_ECR);
            string portServerECR = (data.Port_Server_ECR[0].ConvertToHex() + data.Port_Server_ECR[1].ConvertToHex()).ConvertToPort();

            dtgCmd0x09.Rows.Add(false, sessionID, ipECR, ipServer, portServer, ssid, ssidLength, key, keyLength, modeECR, ipServerECR, portServerECR);
        }

        private void UpdateRow0x09(ClientSession session, Data0x09 data)
        {
            var row = dtgCmd0x09.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[COLUMN_NAME_SESSIONID_2]?.Value?.ToString()) &&
                                                                        session.SessionID == (x.Cells[COLUMN_NAME_SESSIONID_2]?.Value?.ToString()));

            if (row != null)
            {
                string ipECR = ConvertToIPAddress(data.IP_ECR);
                string ipServer = ConvertToIPAddress(data.IP_Server);
                string portServer = (data.Port_Server[0].ConvertToHex() + data.Port_Server[1].ConvertToHex()).ConvertToPort();
                string ssid = data.SSID.UnsafeAsciiBytesToString(0);
                string ssidLength = data.SSID_Length.ToString();
                string key = data.Key.UnsafeAsciiBytesToString(0);
                string keyLength = data.Key_Length.ToString();
                string modeECR = data.Mode_ECR.ToString();
                string ipServerECR = ConvertToIPAddress(data.IP_Server_ECR);
                string portServerECR = (data.Port_Server_ECR[0].ConvertToHex() + data.Port_Server_ECR[1].ConvertToHex()).ConvertToPort();

                row.Cells[COLUMN_NAME_IP_ECR].Value = ipECR;
                row.Cells[COLUMN_NAME_IP_SERVER].Value = ipServer;
                row.Cells[COLUMN_NAME_PORT_SERVER].Value = portServer;
                row.Cells[COLUMN_NAME_SSID].Value = ssid;
                row.Cells[COLUMN_NAME_SSID_LENGTH].Value = ssidLength;
                row.Cells[COLUMN_NAME_KEY].Value = key;
                row.Cells[COLUMN_NAME_KEY_LENGTH].Value = keyLength;
                row.Cells[COLUMN_NAME_MODE_ECR].Value = modeECR;
                row.Cells[COLUMN_NAME_IP_SERVER_ECR].Value = ipServerECR;
                row.Cells[COLUMN_NAME_PORT_SERVER_ECR].Value = portServerECR;
            }
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

        private bool ValidateData0x09(int rowIndex)
        {
            bool isValid = true;

            //IP của ECR
            var ipECR = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_ECR].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_ECR].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(ipECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_ECR].ErrorText = "IP không được trống";
                isValid = false;
            }
            else
            {
                if (!ValidIPAddress(ref ipECR))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_ECR].ErrorText = "IP không hợp lệ";
                    isValid = false;
                }
                else
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_ECR].Value = ipECR;
                }
            }

            //IP của Server
            var ipServer = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(ipServer))
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER].ErrorText = "IP không được trống";
                isValid = false;
            }
            else
            {
                if (!ValidIPAddress(ref ipServer))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER].ErrorText = "IP không hợp lệ";
                    isValid = false;
                }
                else
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER].Value = ipServer;
                }
            }

            //Port của Server
            var portServer = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(portServer))
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER].ErrorText = "Port không được trống";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(portServer, out int result))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER].Value = int.MaxValue;
                }
            }


            //SSID
            var ssid = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID].ErrorText = string.Empty;
            if (!string.IsNullOrEmpty(ssid))
            {
                if (ssid.Length > 32)
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID].ErrorText = "Độ dài vượt quá 32 ký tự";
                    isValid = false;
                }

                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID_LENGTH].Value = ssid.Length;
            }
            else
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_SSID].ErrorText = "Không được trống";
            }

            //KEY
            var key = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY].ErrorText = string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                if (key.Length > 16)
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY].ErrorText = "Độ dài vượt quá 16 ký tự";
                    isValid = false;
                }

                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY_LENGTH].Value = key.Length;
            }
            else
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_KEY].ErrorText = "Không được trống";
            }

            //Chế độ ECR
            var modeECR = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_MODE_ECR].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_MODE_ECR].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(modeECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_MODE_ECR].ErrorText = "Không được trống";
                isValid = false;
            }
            else
            {
                if (!modeECR.Equals("0") && !modeECR.Equals("1"))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_MODE_ECR].Value = "0";
                }
            }

            //IP Server của ECR
            var ipServerECR = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(ipServerECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].ErrorText = "IP không được trống";
                isValid = false;
            }
            else
            {
                if (!ValidIPAddress(ref ipServerECR))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].ErrorText = "IP không hợp lệ";
                    isValid = false;
                }
                else
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_IP_SERVER_ECR].Value = ipServerECR;
                }
            }

            //Port Server của ECR
            var portServerECR = dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER_ECR].Value?.ToString();
            dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER_ECR].ErrorText = string.Empty;
            if (string.IsNullOrEmpty(portServerECR))
            {
                dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER_ECR].ErrorText = "Port không được trống";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(portServerECR, out int result))
                {
                    dtgCmd0x09.Rows[rowIndex].Cells[COLUMN_NAME_PORT_SERVER_ECR].Value = int.MaxValue;
                }
            }

            return isValid;
        }

        private bool CheckRowSelect(DataGridView dtg)
        {
            bool isHaveCarChoose = false;

            var row = dtg.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => x.Cells[DataGridViewCustom.COLUMN_NAME_CHECKBOX].Value != null && Convert.ToBoolean(x.Cells[DataGridViewCustom.COLUMN_NAME_CHECKBOX].Value));
            if (row != null)
            {
                isHaveCarChoose = true;
            }

            if (!isHaveCarChoose)
            {
                MessageBox.Show("Bạn phải chọn xe để thực hiện!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return isHaveCarChoose;
        }

        private bool IsRowSelected(string sessionID, DataGridView dtg, string columnName)
        {
            var row = dtg.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[columnName]?.Value?.ToString()) &&
                                                                        sessionID == (x.Cells[columnName]?.Value?.ToString()));

            if (row == null) return false;

            var isSelected = row.Cells[DataGridViewCustom.COLUMN_NAME_CHECKBOX].Value;

            return isSelected != null ? Convert.ToBoolean(isSelected) : false;
        }

        private void RemoveRowGridView(string sessionID)
        {
            //Remove thông tin xe
            var rowXe = dtgXe.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()) &&
                                                                           sessionID == (x.Cells[COLUMN_NAME_SESSIONID]?.Value?.ToString()));
            if (rowXe != null)
            {
                dtgXe.Rows.Remove(rowXe);
            }

            //Remove thông tin cmd0x09
            var row0x09 = dtgCmd0x09.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Cells[COLUMN_NAME_SESSIONID_2]?.Value?.ToString()) &&
                                                                           sessionID == (x.Cells[COLUMN_NAME_SESSIONID_2]?.Value?.ToString()));
            if (row0x09 != null)
            {
                dtgCmd0x09.Rows.Remove(row0x09);
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

        private byte[] GetDataIPAddress(string ipAddress)
        {
            var arr = ipAddress.Split('.');
            byte[] ipArr = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                ipArr[i] = Convert.ToByte(arr[i]);
            }

            return ipArr;
        }

        private bool ValidIPAddress(ref string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) return false;

            var arr = ipAddress.Split('.');
            if (arr.Length != 4) return false;

            if (!int.TryParse(arr[0], out int arr_0) || arr_0 > 255) arr[0] = "255";
            if (!int.TryParse(arr[1], out int arr_1) || arr_1 > 255) arr[1] = "255";
            if (!int.TryParse(arr[2], out int arr_2) || arr_2 > 255) arr[2] = "255";
            if (!int.TryParse(arr[3], out int arr_3) || arr_3 > 255) arr[3] = "255";

            ipAddress = string.Format("{0}.{1}.{2}.{3}", arr[0], arr[1], arr[2], arr[3]);

            return true;
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
