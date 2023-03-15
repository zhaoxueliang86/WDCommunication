using System.Net.Sockets;

namespace WDCommunication.WDTcp
{
    /// <summary>
    /// 登录的客户端
    /// </summary>
    public class OnlineClient : AClient
    {
        /// <summary>
        /// 推送数据
        /// </summary>
        public event Action<OnlineClient, byte[]>? Push;

        private readonly List<byte[]> DataList = new();
        private long TotalLength = 0;
        private readonly System.Timers.Timer timer = new(20);
        private readonly static object LockObj = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Client"></param>
        public OnlineClient(TcpClient Client) : base(Client)
        {
            timer = new System.Timers.Timer(5);
            timer.Elapsed += Timer_Elapsed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="ReadDuration"></param>
        public OnlineClient(TcpClient Client, int ReadDuration) : base(Client)
        {
            timer = new System.Timers.Timer(ReadDuration);
            timer.Elapsed += Timer_Elapsed;
        }

        public void AppendToCache(byte[] data)
        {
            timer.Stop();
            lock (LockObj)
            {
                DataList.Add(data);
                TotalLength += data.Length;
            }
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            byte[] Result = new byte[TotalLength];
            TotalLength = 0;
            int i = 0;
            lock (LockObj)
            {
                foreach (var data in DataList)
                {
                    data.CopyTo(Result, i);
                    i += data.Length;
                }
                DataList.Clear();
            }
            if (i > 0) Push?.Invoke(this, Result);
        }
    }
}
