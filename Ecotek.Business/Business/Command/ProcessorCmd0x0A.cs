using Ecotek.Common;
using Ecotek.Common.Extentions;
using Ecotek.Common.SupperSocket;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecotek.Business.Business.Command
{
    public class ProcessorCmd0x0A : CommandBase
    {
        public delegate void ThongSoDelegate(string sessionID, int frameSendCount, int frameReceiveCount, int frameTotal, string state);
        public event ThongSoDelegate OnUpdateThongSo;

        public List<Frame> Frames { set; get; } = new List<Frame>();

        private ConcurrentDictionary<ClientSession, Data0x0A> _dic0x0A { set; get; } = new ConcurrentDictionary<ClientSession, Data0x0A>();

        public override void SendMsgAll(List<ClientSession> sessions)
        {
            if (Server != null)
            {
                Parallel.ForEach(sessions, session =>
                {
                    if (!_dic0x0A.ContainsKey(session))
                    {
                        _dic0x0A.TryAdd(session, new Data0x0A() 
                        {
                            Index = 0,
                            Frames = Frames,
                            ThongSoFrame = new ThongSoFrame() { FrameTotal = Frames.Count }
                        });
                    }
                    else
                    {
                        _dic0x0A[session].Index = 0;
                        _dic0x0A[session].Frames = Frames;
                        _dic0x0A[session].ThongSoFrame.FrameSendCount = 0;
                        _dic0x0A[session].ThongSoFrame.FrameReceiveCount = 0;
                        _dic0x0A[session].ThongSoFrame.FrameTotal = Frames.Count;
                        _dic0x0A[session].ThongSoFrame.State = string.Empty;
                    }

                    OnUpdateThongSo(session.SessionID, 0, 0, Frames.Count, string.Empty);

                    Process0x0A(session);
                });
            }    
        }

        public void Process0x0A(ClientSession session, bool isSend = true)
        {
            if (_dic0x0A.ContainsKey(session))
            {
                if (_dic0x0A[session].Frames != null && _dic0x0A[session].Frames.Count > 0)
                {
                    if (!isSend)
                    {
                        _dic0x0A[session].ThongSoFrame.FrameReceiveCount++;
                        var thongSo = _dic0x0A[session].ThongSoFrame;

                        OnUpdateThongSo(session.SessionID, thongSo.FrameSendCount, thongSo.FrameReceiveCount, thongSo.FrameTotal, thongSo.State);
                    }    

                    int index = _dic0x0A[session].Index;

                    Frame frame = new Frame()
                    {
                        Code = Constants.Hx0A,
                    };

                    byte soXe = Constants.Hx01;

                    if (index < _dic0x0A[session].Frames.Count)
                    {
                        frame.Data = new byte[_dic0x0A[session].Frames[index].Data.Length + 5];
                        frame.Data[0] = soXe;//Số xe
                        frame.Data[1] = Constants.Hx00;//N1
                        frame.Data[2] = Constants.Hx00;//N2
                        frame.Data[3] = (byte)index.LengthLow();//byte thấp chỉ số frame
                        frame.Data[4] = (byte)index.LengthHigh();//byte cao chỉ số frame
                        for (int k = 5; k < _dic0x0A[session].Frames[index].Data.Length + 5; k++)
                        {
                            frame.Data[k] = _dic0x0A[session].Frames[index].Data[k - 5];
                        }
                    }
                    else if (index == _dic0x0A[session].Frames.Count) //Frame nhận biết hết dữ liệu
                    {
                        frame.Data = new byte[5];
                        frame.Data[0] = soXe;//Số xe
                        frame.Data[1] = Constants.Hx00;//N1
                        frame.Data[2] = Constants.Hx00;//N2
                        frame.Data[3] = Constants.HxFF;//byte thấp chỉ số frame
                        frame.Data[4] = Constants.HxFF;//byte cao chỉ số frame
                    }

                    if (index <= _dic0x0A[session].Frames.Count)
                    {
                        Server.SendMsg(session, frame.Get());

                        _dic0x0A[session].ThongSoFrame.FrameSendCount++;
                        var thongSo = _dic0x0A[session].ThongSoFrame;

                        OnUpdateThongSo(session.SessionID, thongSo.FrameSendCount, thongSo.FrameReceiveCount, thongSo.FrameTotal, thongSo.State);

                        _dic0x0A[session].Index++;
                        if (index == _dic0x0A[session].Frames.Count)
                        {
                            _dic0x0A[session].Index = 0;
                        }
                    }
                }
            }
        }
    }
}
