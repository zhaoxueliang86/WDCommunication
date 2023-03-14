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
        public event DelegateDisconnect? EventDisconnect;
        public delegate void DelegateDisconnect(object sender, IPEndPoint ServerIpPoint);

        /// <summary>
        /// 发送数据的事件
        /// </summary>
        public event DelegateAfterSend? EventAfterSend;
        public delegate void DelegateAfterSend(object sender, DataEventAges e);

        /// <summary>
        /// 接收数据之后的事件
        /// </summary>
        public event DelegateAfterReceive? EventAfterReceive;
        public delegate void DelegateAfterReceive(object sender, DataEventAges e);
        #endregion

        public Client(IPEndPoint IpEndPoint)
        {
            ServerIpPoint = IpEndPoint;
            EventConnection += Client_OnConnection;
            EventDisconnect += Client_OnDisconnect;
            TimerConnect.Interval = 3000;
            TimerConnect.Elapsed += TimerConnect_Elapsed;
        }

        private void TimerConnect_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Connect();
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
            failCount = 0;
            if (TimerConnect.Enabled) TimerConnect.Stop();
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
                            EventAfterReceive?.Invoke(this, new DataEventAges(ServerIpPoint, DataEventAges.EnumDataType.Receive, data));
                        }
                    }
                }
                catch
                {
                    TcpClient.Close();
                    EventDisconnect?.Invoke(this, ServerIpPoint);
                }
            }, TokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        private TcpClient TcpClient = new();
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
        /// 重连计时器
        /// </summary>
        private System.Timers.Timer TimerConnect = new();
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer TimerHead = new();
        private int failCount = 0;

        /// <summary>
        /// 连接服务端方法
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            try
            {
                TcpClient = new();
                TcpClient.Connect(ServerIpPoint);
                Connection = TcpClient.Connected;
            }
            catch
            {
                Connection = false;
                failCount++;
                if (!TimerConnect.Enabled) TimerConnect.Start();
            }
            finally
            {
                EventConnection?.Invoke(this, ServerIpPoint, Connection);
            }
        }
        /// <summary>
        /// 连接服务端方法
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                TcpClient = new();
                await TcpClient.ConnectAsync(ServerIpPoint);
                Connection = TcpClient.Connected;
            }
            catch
            {
                Connection = false;
            }
            finally {
                EventConnection?.Invoke(this, ServerIpPoint, Connection);
            }
            return Connection;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            TcpClient.Close();
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
                try
                {
                    var NetStream = TcpClient.GetStream();
                    await NetStream.WriteAsync(data);

                    EventAfterSend?.Invoke(this, new DataEventAges(ServerIpPoint, DataEventAges.EnumDataType.Send, data));
                }
                catch
                {
                    
                }
            }
        }

        /// <summary>
        /// 资源回收
        /// </summary>
        public void Dispose()
        {
        }
    }
}
