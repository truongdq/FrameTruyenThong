namespace Ecotek.Common.SupperSocket
{
    public enum Command
    {
        /// <summary>
        /// Default state
        /// </summary>
        None = 0,

        /// <summary>
        /// ID check
        /// </summary>
        ID_Check,
        
        /// <summary>
        /// ID check succeeded 0x02
        /// </summary>
        ID_Check_Ok,

        ///
        /// Server xac nhan frame picture 0x03
        ///
        ID_REPEAT_INDEX_PICTURE,

        ///
        /// Read SD Card data thi sinh thi 0x04
        /// 
        ID_READ_LOG_DATA,
        // Sau khi gui yeu cau doc du lieu, ECR gui du lieu len voi ID = 4
        // Sau khi ket thuc gui du lieu, ECR gui tiep anh cua thi sinh len.
        ///
        /// Start Frame Picture save in SD card. 0x05
        /// ECR gui thong tin anh thi sinh len server
        ///
        ID_READE_SD_PICTURE,

        /// 
        /// ECR gui du lieu thi cua thi sinh len trung tam 0x06
        /// 
        ID_GET_DATA_THI_SH,

        /// <summary>
        /// SERVER ra lenh chup anh tu trung tam  0x07
        /// Them byte lan thi cua thi sinh
        /// </summary>
        ID_REQUESR_TAKE_PHOTO,
        /// <summary>
        /// Doc thong so ECR
        /// </summary>
        ID_READ_ECR_CONFIG,
        ///
        /// Cai dat thong so ECR
        /// 
        ID_SETUP_ECR_CONFIG,
        /// <summary>
        /// There is a connected user.
        /// </summary>
        User_Connect,
        /// <summary>
        /// There is a user who has disconnected.
        /// </summary>
        User_Disonnect,
        /// <summary>
        /// Sends a list of users.
        /// </summary>
        User_List,
        /// <summary>
        /// Requests to update the user list.
        /// </summary>
        User_List_Get,

        /// <summary>
        /// Called after ID idiomatic is checked
        /// </summary>
        Login,
        /// <summary>
        /// Notify the client that the server has completed all login processes
        /// </summary>
        Login_Complete,
        /// <summary>
        /// Log out
        /// </summary>
        Logout,

        /// <summary>
        /// Send Message
        /// </summary>
        Msg,

        /// <summary>
        /// Start large file transfer.
        /// Sender sends information about the file to send.
        /// </summary>
        LargeData_Start,
        /// <summary>
        /// File index information
        /// Generates an index to identify the file to which the recipient receives the file, and tells this index to the sender.
        /// </summary>
        LargeData_Info,
        /// <summary>
        /// Send the truncated file.
        /// </summary>
        LargeData_Receive,
        /// <summary>
        /// We inform the sending side that we receive one piece of cut data from the receiving side.
        /// </summary>
        LargeData_Receive_Complete,
        /// <summary>
        /// I tried to transfer all the cropped files.
        /// </summary>
        LargeData_End,
    }
}
