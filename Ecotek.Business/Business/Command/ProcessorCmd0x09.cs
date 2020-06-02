using Ecotek.Common;
using Ecotek.Common.SupperSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecotek.Business.Business.Command
{
    public class ProcessorCmd0x09 : CommandBase
    {
        public List<KeyValuePair<ClientSession, Data0x09>> Data { set; get; }

        public override void SendMsg()
        {
            if (Server != null && Data != null)
            {
                Parallel.ForEach(Data, x =>
                {
                    Frame frame = AnalysisData(x.Value);

                    Server.SendMsg(x.Key, frame.Get());
                });
            }
        }

        public Frame AnalysisData(Data0x09 data)
        {
            Frame frame = new Frame()
            {
                Code = Constants.Hx09,
                Data = new byte[data.Length]
            };

            int indexOffArr = 0;

            //4 byte IP ECR
            for (int i = 0; i < data.IP_ECR.Length; i++)
            {
                frame.Data[indexOffArr] = data.IP_ECR[i];
                indexOffArr++;
            }

            //4 Byte IP SERVER
            for (int i = 0; i < data.IP_Server.Length; i++)
            {
                frame.Data[indexOffArr] = data.IP_Server[i];
                indexOffArr++;
            }

            //2 Byte Port SERVER
            for (int i = 0; i < data.Port_Server.Length; i++)
            {
                frame.Data[indexOffArr] = data.Port_Server[i];
                indexOffArr++;
            }

            //1 Byte độ dài SSID
            frame.Data[indexOffArr] = data.SSID_Length;
            indexOffArr++;

            //32 Byte SSID
            for (int i = 0; i < data.SSID.Length; i++)
            {
                frame.Data[indexOffArr] = data.SSID[i];
                indexOffArr++;
            }

            //1 Byte độ dài KEY
            frame.Data[indexOffArr] = data.Key_Length;
            indexOffArr++;

            //16 byte KEY
            for (int i = 0; i < data.Key.Length; i++)
            {
                frame.Data[indexOffArr] = data.Key[i];
                indexOffArr++;
            }

            //1 Byte chế độ ECR (ECR = 0, ECR_RF = 1)
            frame.Data[indexOffArr] = data.Mode_ECR;
            indexOffArr++;

            //4 Byte IP SERVER của ECR kết nối với tủ trung tâm trong chế độ tập tự do không chạy phần mềm trên máy tính
            for (int i = 0; i < data.IP_Server_ECR.Length; i++)
            {
                frame.Data[indexOffArr] = data.IP_Server_ECR[i];
                indexOffArr++;
            }

            //2 Byte Port SERVER của ECR kết nối với tủ trung tâm trong chế độ tập tự do không chạy phần mềm trên máy tính
            for (int i = 0; i < data.Port_Server_ECR.Length; i++)
            {
                frame.Data[indexOffArr] = data.Port_Server_ECR[i];
                indexOffArr++;
            }

            return frame;
        }
    }
}
