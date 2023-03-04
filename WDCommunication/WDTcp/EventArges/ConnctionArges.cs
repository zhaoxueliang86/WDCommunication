using System.Net;

namespace WDCommunication.WDTcp
{
    public class ConnectionEventAges : EventArgs
    {
        public ConnectionEventAges(IPEndPoint IpEndPoint, bool Connection)
        {
            this.IpEndPoint = IpEndPoint;
            this.Connection = Connection;
        }
        public IPEndPoint IpEndPoint { get; }
        public bool Connection { get; }

        public IPAddress IpAddress { get => IpEndPoint.Address; }
        public int Port { get => IpEndPoint.Port; }
        public string Ip_Port => $"{IpEndPoint}";
    }
}
