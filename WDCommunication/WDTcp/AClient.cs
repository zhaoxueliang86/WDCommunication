using System.Net;
using System.Net.Sockets;

namespace WDCommunication.WDTcp
{
    /// <summary>
    /// 登录的客户端
    /// </summary>
    public abstract class AClient : EventArgs, IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Client"></param>
        public AClient(TcpClient Client)
        {
            this.Client = Client;
            this.IpEndPoint = (IPEndPoint)Client.Client.RemoteEndPoint!;
        }
        public AClient(System.Net.IPAddress IpAddress, int Port)
        {
            this.IpEndPoint = new(IpAddress, Port);
            this.Client = new TcpClient(IpEndPoint);
        }
        /// <summary>
        /// TcpClient
        /// </summary>
        public TcpClient Client { get; }
        /// <summary>
        /// Ip终结点
        /// </summary>
        public IPEndPoint IpEndPoint { get; private set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime OnlineDatetime { get; } = DateTime.Now;
        /// <summary>
        /// 客户端Ip地址
        /// </summary>
        public System.Net.IPAddress IpAddress { get => IpEndPoint.Address; }
        /// <summary>
        /// 客户端端口号
        /// </summary>
        public int Port { get => IpEndPoint.Port; }
        /// <summary>
        /// 客户端，Ip:Port
        /// </summary>
        public string IP_Port
        {
            get => IpEndPoint.ToString();
        }
        /// <summary>
        /// 资源回收
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
