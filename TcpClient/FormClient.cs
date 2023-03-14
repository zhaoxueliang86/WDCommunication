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

        private void TcpClient_OnConnection(object? sender, System.Net.IPEndPoint ServerIpPoint, bool success)
        {
            BeginInvoke(() =>
            {
                BtnStart.Enabled = !success;
                BtnStop.Enabled = !BtnStart.Enabled;
                BtnSend.Enabled = !BtnStart.Enabled;
                Total = 0;
                rReceive.AppendText($"【{txtIp.Text}:{txtPort.Text}】Connection {(success ? "success" : "fail")}{Environment.NewLine}");
            });
        }

        private void TcpClient_OnDisconnect(object? sender, System.Net.IPEndPoint ServerIpPoint)
        {
            BeginInvoke(() =>
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = !BtnStart.Enabled;
                BtnSend.Enabled = !BtnStart.Enabled;
                rReceive.AppendText($"【{txtIp.Text}:{txtPort.Text}】Disconnect{Environment.NewLine}");
            });
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            BtnStart.Enabled = false;
            TcpClient = new Client(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(txtIp.Text), int.Parse(txtPort.Text)));
            TcpClient.EventConnection += TcpClient_OnConnection;
            TcpClient.EventDisconnect += TcpClient_OnDisconnect;
            TcpClient.EventAfterReceive += TcpClient_OnAfterReceive;
            rReceive.AppendText($"【{txtIp.Text}:{txtPort.Text}】Connecting...{Environment.NewLine}");
            TcpClient.Connect();
        }

        private void TcpClient_OnAfterReceive(object? sender, WDCommunication.WDTcp.DataEventAges e)
        {
            BeginInvoke(() =>
            {
                Print($"【{++Total}】.【{e.IPAddress}:{e.Port}】: {Encoding.ASCII.GetString(e.Data)}{Environment.NewLine}");
            });
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            TcpClient!.Disconnect();
        }

        private void BtnSend_Click(object? sender, EventArgs e)
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

        private System.Timers.Timer timer = new();
        private void BtnStart1_Click(object sender, EventArgs e)
        {
            TcpClient = new Client(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(txtIp.Text), int.Parse(txtPort.Text)));
            TcpClient.EventConnection += TcpClient_EventConnection;
            TcpClient.EventDisconnect += TcpClient_EventDisconnect;
            TcpClient.EventAfterReceive += TcpClient_OnAfterReceive;
            TcpClient.ConnectAsync().ConfigureAwait(false);
            timer.Interval = int.Parse(textBox1.Text) * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void TcpClient_EventDisconnect(object sender, System.Net.IPEndPoint ServerIpPoint)
        {
            BeginInvoke(() =>
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = !BtnStart.Enabled;
                BtnSend.Enabled = !BtnStart.Enabled;
            });
            if (!timer.Enabled) timer.Start();
        }

        private void TcpClient_EventConnection(object sender, System.Net.IPEndPoint ServerIpPoint, bool success)
        {
            BeginInvoke(() =>
            {
                BtnStart.Enabled = !success;
                BtnStop.Enabled = !BtnStart.Enabled;
                BtnSend.Enabled = !BtnStart.Enabled;
                Total = 0;
                if (success) timer.Stop();
            });
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _ = TcpClient!.ConnectAsync();
        }
    }
}
