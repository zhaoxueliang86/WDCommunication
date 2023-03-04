using System.Text;
using WDCommunication.WDTcp;

namespace TcpClient
{
    public partial class FormClient : Form
    {
        private WDCommunication.WDTcp.Client? TcpClient;
        private int Total = 0;
        public FormClient()
        {
            InitializeComponent();
        }

        private void TcpClient_OnConnection(object sender, System.Net.IPEndPoint ServerIpPoint, bool success)
        {
            BeginInvoke(() =>
            {
                BtnStart.Enabled = !success;
                BtnStop.Enabled = !BtnStart.Enabled;
                BtnSend.Enabled = !BtnStart.Enabled;
                Total = 0;

            });
        }

        private void TcpClient_OnDisconnect(object sender, System.Net.IPEndPoint ServerIpPoint)
        {
            BeginInvoke(() =>
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = !BtnStart.Enabled;
                BtnSend.Enabled = !BtnStart.Enabled;
            });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            TcpClient = new Client(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(txtIp.Text), int.Parse(txtPort.Text)));
            TcpClient.EventConnection += TcpClient_OnConnection;
            TcpClient.EventDisconnect += TcpClient_OnDisconnect;
            TcpClient.EventAfterReceive += TcpClient_OnAfterReceive;
            TcpClient.ConnectAsync().ConfigureAwait(false);
        }

        private void TcpClient_OnAfterReceive(object sender, WDCommunication.WDTcp.DataEventAges e)
        {
            BeginInvoke(() =>
            {
                Print($"【{++Total}】.【{e.IPAddress}:{e.Port}】: {Encoding.ASCII.GetString(e.Data)}{Environment.NewLine}");
            });
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            TcpClient!.DisconnectAsync().ConfigureAwait(false);
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(rSend.Text);
            _ = TcpClient!.SendAsync(data);
        }

        private void Print(string text)
        {
            BeginInvoke(() =>
            {
                rReceive.AppendText(text);
                rReceive.ScrollToCaret();
            });
        }
    }
}
