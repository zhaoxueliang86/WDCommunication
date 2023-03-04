using System.Net;
using System.Net.Sockets;

namespace WDCommunication.WDTcp
{
    public class Client : IDisposable
    {
        #region 定义事件
        /// <summary>
        /// 服务端已连接
        /// </summary>
        public event DelegateConnection? EventConnection;
        public delegate void DelegateConnection(object sender, IPEndPoint ServerIpPoint, bool success);
        /// <summary>
        /// 服务端已断开
        /// </summary>
        public event DelegateDisconnect? OnDisconnect;
        public delegate void DelegateDisconnect(object sender, IPEndPoint ServerIpPoint);

        /// <summary>
        /// 发送数据的事件
        /// </summary>
        public event DelegateAfterSend? OnAfterSend;
        public delegate void DelegateAfterSend(object sender, DataEventAges e);

        /// <summary>
        /// 接收数据之后的事件
        /// </summary>
        public event DelegateAfterReceive? OnAfterReceive;
        public delegate void DelegateAfterReceive(object sender, DataEventAges e);
        #endregion

        public Client(IPEndPoint IpEndPoint)
        {
            this.ServerIpPoint = IpEndPoint;
            EventConnection += Client_OnConnection;
            OnDisconnect += Client_OnDisconnect;
        }

        private void Client_OnDisconnect(object sender, IPEndPoint ServerIpPoint)
        {
            TokenSource.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ServerIpPoint"></param>
        private void Client_OnConnection(object sender, IPEndPoint ServerIpPoint, bool success)
        {
            if (!success) return;
            TokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    NetworkStream NetStream = TcpClient.GetStream();
                    byte[] buffer = new byte[1024];
                    while (!TokenSource.Token.IsCancellationRequested)
                    {
                        int len = NetStream.Read(buffer);
                        if (len > 0)
                        {
                            byte[] data = buffer.Take(len).ToArray();
                            OnAfterReceive?.Invoke(this, new DataEventAges(ServerIpPoint, DataEventAges.EnumDataType.Receive, data));
                        }
                    }
                }
                catch
                {
                    OnDisconnect?.Invoke(this, ServerIpPoint);
                }
            }, TokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        private readonly TcpClient TcpClient = new();
        private CancellationTokenSource TokenSource = new();
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Connection { get; private set; } = false;
        /// <summary>
        /// 服务端Ip终结点
        /// </summary>
        public IPEndPoint ServerIpPoint { get; private set; }
        /// <summary>
        /// 服务端Ip地址
        /// </summary>
        public IPAddress ServerIpAddress { get => ServerIpPoint.Address; }
        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort { get => ServerIpPoint.Port; }
        /// <summary>
        /// 连接服务端方法
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                await TcpClient.ConnectAsync(ServerIpPoint);
                Connection = true;
                EventConnection?.Invoke(this, ServerIpPoint, true);
                return true;
            }
            catch
            {
                Connection = false;
                EventConnection?.Invoke(this, ServerIpPoint, false);
                return false;
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public async Task DisconnectAsync()
        {
            await Task.Run(() =>
            {
                TcpClient.Close();
                OnDisconnect?.Invoke(this, ServerIpPoint);
            });
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendAsync(byte[] data)
        {
            if (TcpClient.Connected)
            {
                var NetStream = TcpClient.GetStream();
                await NetStream.WriteAsync(data);
                OnAfterSend?.Invoke(this, new DataEventAges(ServerIpPoint, DataEventAges.EnumDataType.Send, data));
            }
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
