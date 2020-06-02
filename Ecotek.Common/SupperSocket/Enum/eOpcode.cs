namespace Ecotek.Common.SupperSocket
{
    public enum eOpcode
    {
        D_CMSG_AUTH_IMEI = 0x00,
        SMSG_AUTH_CHALLENGE = 0x01,
        CMSG_AUTH_CHALLENGE = 0x02,
        SMSG_AUTH_PROOF = 0x03,
        CMSG_AUTH_PROOF = 0x04,
        D_SMSG_AUTH_ACCEPT = 0x05,
        D_CMSG_DATA = 0x06,
        D_SMSG_DATA = 0x07,
        D_CMSG_PICTURE_DATA = 0x08,
        D_SMSG_PICTURE_DATA = 0x09,

        D_CMSG_SENDDATETIME = 0x10,//GUI THOI GIAN TU TRUNG TAM XUONG THIET BI
        D_SMSG_RECVDATETIME = 0x11,//NHAN THOI GIAN TU THIET BI LEN TRUNG TAM

        D_CMSG_SENDRETEST = 0x14,//Xac nhan danh sach thi sinh thi lai
        D_SMSG_SENDRETEST = 0x15,//Gui danh sach thi sinh thi lai

        D_CMSG_DISTEST = 0x16,//Xac nhan danh sach thi sinh thi lai
        D_SMSG_DISTEST = 0x17,//Gui danh sach thi sinh thi lai



        D_SMSG_CONTESTANT_LIST = 0x0B,
        D_CMSG_CONTESTANT_LIST = 0x0A,

        /// <summary>
        /// Thiết bị trả về dữ liệu thi của thi sinh
        /// </summary>
        D_CMSG_QUERY_CONTESTANT_INFO = 0x0C,
        /// <summary>
        /// Đọc dữ liệu thi của 1 thí sinh
        /// Mã lệnh server truyền xuống là 0x0D, dữ liệu gồm: 1 byte năm, 1 byte tháng, 1 byte ngày, 2 byte khóa thi (byte thấp trước, byte cao sau), 2 byte số báo danh (byte thấp trước, byte cao sau).
        /// </summary>
        D_SMSG_QUERY_CONTESTANT_INFO = 0x0D,
        D_CMSG_QUERY_EXAMINATION_LIST = 0x0E,
        /// <summary>
        ///- Frame server truyền xuống để đọc với mã lệnh là 0x0F, dữ liệu 3 byte: Năm, tháng, ngày cần đọc.
        ///- Frame thiết bị phản hồi với mã lệnh là 0x0E, nếu không có dữ liệu thì phần dữ liệu là 1 byte = 0x0E. Nếu có dữ liệu thì gồm các nhóm 4 byte: 2 byte đầu là khóa thi (Byte thấp trước, Byte cao sau), 2 byte sau là số báo danh (Byte thấp trước, Byte cao sau). Không cần server xác nhận và ở dưới sẽ truyền lên đến hết thì thôi.
        /// </summary>
        D_SMSG_QUERY_EXAMINATION_LIST = 0x0F,

        D_CMSG_PICTURE_DATA1 = 0x18,
        D_SMSG_PICTURE_DATA1 = 0x19,

        D_CMSG_PICTURE_DATA2 = 0x1A,
        D_SMSG_PICTURE_DATA2 = 0x1B,

        D_CMSG_SET_DEVICEPARAMETERS = 0x1C,
        D_SMSG_SET_DEVICEPARAMETERS = 0x1D,

        D_CMSG_READ_DEVICEPARAMETERS = 0x1E,
        D_SMSG_READ_DEVICEPARAMETERS = 0x1F,

        D_CMSG_SET_DEVICEPARAMETERS_MR = 0x22,
        D_SMSG_SET_DEVICEPARAMETERS_MR = 0x23,

        D_CMSG_READ_DEVICEPARAMETERS_MR = 0x24,
        D_SMSG_READ_DEVICEPARAMETERS_MR = 0x25,

        D_SMSG_CALLBACK_CHISO = 0x26,// DOC LAI DU LIEU THEO CHI SO SU KIEN



        //Bắt đầu nạp từ xa trung tâm gửi xuống cho thiết bị mã lệnh 0xFB, dữ liệu gồm 4 byte là dung lượng file (byte thấp trước, byte cao sau)
        //- Khi thiết bị nhận được mã lệnh 0xFB từ trung tâm, thì thiết bị sẽ gửi lên trung tâm mã lệnh 0xFC, dữ liệu gồm 2 byte là chỉ số của  gói dữ liệu nạp (bắt đầu = 1, byte thấp trước, byte cao sau).
        //- Khi trung tâm được mã lệnh 0xFC từ thiết bị thì trung tâm gửi xuống frame dữ liệu nạp với mã lệnh 0xFC, dữ liệu gồm: 2 byte đầu là chỉ số của gói dữ liệu nạp mà thiết bị yêu cầu (byte thấp trước, byte cao sau), 900 byte dữ liệu nạp (nêu frame cuối mà không đủ 900 byte thì điền số byte còn lại là 0xFF).
        //- Khi thiết bị nhận đủ dữ liệu nạp thì thiết bị sẽ yêu cầu trung tâm kết thúc nạp bằng mã lệnh 0xFD, khi trung tâm nhận được mã lệnh này thì sẽ có thời gian là 1 phút để xác nhận kết thúc nạp bằng mã lệnh 0xFD, dữ liệu 1 byte (= 0 không chấp nhận dữ liệu vừa nạp xuống, = 1 là chấp nhận dữ liệu nạp xuống)
        D_SMSG_CONFIG = 0xFB,
        D_CMSG_CONFIG = 0xFC,

        D_CMSG_FINISH_CONFIG = 0xFD,
        // D_MSG_CONFIG             ,
        // CMSG_PING        = 0xFD ,
        // SMSG_PONG        = 0xFE ,
        MSG_COUNT
    }
}
