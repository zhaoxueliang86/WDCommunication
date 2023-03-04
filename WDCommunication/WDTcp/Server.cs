using System.Net;
using System.Net.Sockets;

namespace WDCommunication.WDTcp
{
    public class Server : IDisposable
    {
        #region 定义事件
        /// <summary>
        /// 服务状态改变
        /// </summary>
        public event EventHandler<StateEventArgs>? EventStateChanged;
        /// <summary>
        /// 客户端已连接
        /// </summary>
        public event EventHandler<OnlineClient>? EventConnection;
        /// <summary>
        /// 客户端已断开
        /// </summary>
        public event EventHandler<OnlineClient>? EventDisconnect;
        /// <summary>
        /// 接收数据之前的事件
        /// </summary>
        public event EventHandler<OnlineClient>? EventBeforeReceive;
        /// <summary>
        /// 接收数据之后的事件
        /// </summary>
        public event EventHandler<DataEventAges>? EventAfterReceive;
        /// <summary>
        /// 发送数据的事件
        /// </summary>
        public event EventHandler<DataEventAges>? EventAfterSend;
        #endregion

        #region 定义私有对象
        private CancellationTokenSource? TokenSource;
        /// <summary>
        /// 监听对象
        /// </summary>
        private TcpListener? Listener;
        /// <summary>
        /// 客户端集合
        /// </summary>
        private readonly List<OnlineClient> ClientCollect = new();
        #endregion

        #region 构造函数
        /// <summary>
        /// 服务端对象的构造函数
        /// </summary>
        public Server() { Id = Guid.NewGuid().ToString(); }
        /// <summary>
        /// 服务端对象的构造函数
        /// </summary>
        /// <param name="IpAddress">IP地址</param>
        /// <param name="Port">端口：0-65535</param>
        public Server(IPAddress IpAddress, int Port)
        {
            Id = Guid.NewGuid().ToString();
            this.IpAddress = IpAddress;
            this.Port = Port;
        }
        /// <summary>
        /// 服务端对象的构造函数
        /// </summary>
        /// <param name="Id">对象的ID</param>
        public Server(string id)
        {
            Id = id;
        }
        /// <summary>
        /// 服务端对象的构造函数
        /// </summary>
        /// <param name="Id">对象的ID</param>
        /// <param name="IpAddress">IP地址</param>
        /// <param name="Port">端口：0-65535</param>
        public Server(string id, IPAddress IpAddress, int Port)
        {
            Id = id;
            this.IpAddress = IpAddress;
            this.Port = Port;
        }
        #endregion

        /// <summary>
        /// 对象的Id
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Ip地址，缺省为Any
        /// </summary>
        public IPAddress IpAddress
        {
            get => _IpAddress;
            set
            {
                if (State == EnumServerStateType.停止)
                {
                    _IpAddress = value;
                }
                else
                {
                    throw new InvalidOperationException("服务运行中不能更改Ip地址");
                }
            }
        }
        private IPAddress _IpAddress = IPAddress.Any;
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port
        {
            get => _Port;
            set
            {
                if (State == EnumServerStateType.停止)
                {
                    _Port = value;
                }
                else
                {
                    throw new InvalidOperationException("服务运行中不能更改端口号");
                }
            }
        }
        private int _Port = 6000;

        /// <summary>
        /// 服务状态
        /// </summary>
        public EnumServerStateType State
        {
            get => _ServerState;
            private set
            {
                if (_ServerState != value)
                {
                    _ServerState = value;
                    //抛出状态改变事件
                    EventStateChanged?.Invoke(this, new StateEventArgs(value));
                }
            }
        }
        private EnumServerStateType _ServerState = EnumServerStateType.停止;
        /// <summary>
        /// 缓冲区，缺省为1K
        /// </summary>
        public long BufferSize { get; set; } = 1024;
        /// <summary>
        /// 启动监听服务
        /// </summary>
        public bool Start()
        {
            if (State == EnumServerStateType.运行) return false;
            //绑定监听端口
            Listener = new TcpListener(IpAddress, Port);
            Listener.Start();
            State = EnumServerStateType.运行;
            TokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => Accept(Listener),
                TokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
            return true;
        }
        /// <summary>
        /// 停止监听服务
        /// </summary>
        public bool Stop()
        {
            if (State == EnumServerStateType.停止) return false;
            TokenSource!.Cancel();
            for (int i = ClientCollect.Count - 1; i >= 0; i--)
            {
                var client = ClientCollect[i];
                ClientCollect.RemoveAt(i);
                client.Client.Close();
                EventDisconnect?.Invoke(this, client);
                client.Dispose();
            }
            Listener?.Stop();
            State = EnumServerStateType.停止;
            return true;
        }
        /// <summary>
        /// 阻塞等待客户端连接
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void Accept(TcpListener server)
        {
            while (!TokenSource!.Token.IsCancellationRequested)
            {
                try
                {
                    //等待接收客户端连接
                    var client = server.AcceptTcpClient();
                    Task.Run(() =>
                    {
                        OnlineClient onlineClient = new(client);
                        if (!ClientCollect.Contains(onlineClient))
                        {
                            //客户端TcpClient添加到字典中
                            ClientCollect.Add(onlineClient);
                            //抛出客户端连上的事件
                            EventConnection?.Invoke(this, onlineClient);
                            onlineClient.Push += OnlineClient_Push;
                        }
                        Receive(onlineClient);
                    }, TokenSource.Token);
                }
                catch (Exception)
                {
                    State = EnumServerStateType.停止;
                    throw;
                }
            }
        }
        /// <summary>
        /// 推送数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="Data"></param>
        private void OnlineClient_Push(OnlineClient client, byte[] Data)
        {
            Task.Run(() =>
            {
                DataEventAges Args = new(client.IpEndPoint, DataEventAges.EnumDataType.Receive, Data);
                EventAfterReceive?.Invoke(this, Args);
            });
        }
        /// <summary>
        /// 接收数据的方法
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private void Receive(OnlineClient client)
        {
            //创建缓冲区
            byte[] buffer = new byte[BufferSize];
            int length;
            try
            {
                while ((length = client.Client.GetStream().Read(buffer, 0, buffer.Length)) > 0)
                {
                    //客户端连接服务器成功后，服务器接收客户端发送的消息
                    client.AppendToCache(buffer.Take(length).ToArray());
                }
                //如果已经断开，则抛出事件
                if (client.Client.Connected) EventDisconnect?.Invoke(this, client);
            }
            catch
            {
                if (client.Client.Connected) EventDisconnect?.Invoke(this, client);
            }
        }
        /// <summary>
        /// 向客户端发送数据
        /// </summary>
        /// <param name="Ip_Port"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync(string Ip_Port, byte[] data)
        {
            var client = ClientCollect.Where((client) => { return client.IP_Port == Ip_Port; }).First();
            if (client == null) return false;
            //var task= await SendAsync(client, data);
            return await SendAsync(client, data);
        }

        /// <summary>
        /// 向客户端发送数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        public async Task<bool> SendAsync(OnlineClient client, byte[] data)
        {
            if (!ClientCollect.Contains(client)) return false;
            var tcpClient = client.Client;
            if (tcpClient.Connected)
            {
                try
                {
                    var NetStream = tcpClient.GetStream();
                    await NetStream.WriteAsync(data, TokenSource!.Token)
                        .ConfigureAwait(false);
                    EventAfterSend?.Invoke(this, new DataEventAges(client.IpEndPoint, DataEventAges.EnumDataType.Send, data));
                    return true;
                }
                catch { return false; }
            }
            else
            {
                return false;
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