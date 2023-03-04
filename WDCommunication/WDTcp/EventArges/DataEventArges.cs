using System.Net;

namespace WDCommunication.WDTcp
{
    /// <summary>
    /// 发送接收数据事件的参数
    /// </summary>
    public class DataEventAges : EventArgs
    {
        /// <summary>
        /// 发送or接收
        /// </summary>
        public enum EnumDataType
        {
            /// <summary>
            /// 发送
            /// </summary>
            Send = 1,
            /// <summary>
            /// 接收
            /// </summary>
            Receive = 2
        }
        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="IpEndPoint"></param>
        /// <param name="DataType"></param>
        /// <param name="data"></param>
        public DataEventAges(IPEndPoint IpEndPoint, EnumDataType DataType, byte[] data) { this.IpEndPoint = IpEndPoint; this.DataType = DataType; Data = data; }
        /// <summary>
        /// Tcp套接字
        /// </summary>
        public IPEndPoint IpEndPoint { get; }
        /// <summary>
        /// 客户端Ip地址
        /// </summary>
        public IPAddress IPAddress
        {
            get => IpEndPoint.Address;

        }
        /// <summary>
        /// 客户端端口号
        /// </summary>
        public int Port
        {
            get => IpEndPoint.Port;
        }
        /// <summary>
        /// 客户端，Ip:Port
        /// </summary>
        public string IP_Port
        {
            get => IpEndPoint.ToString();
        }
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; }
        /// <summary>
        /// 数据的类型，接收还是发送
        /// </summary>
        public EnumDataType DataType { get; }
    }
}
