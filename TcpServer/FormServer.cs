using System.Net;
using System.Text;
using WDCommunication.WDTcp;

namespace TcpServer
{
    public partial class FormServer : Form
    {
        private readonly Server tcpServer = new();

        public FormServer()
        {
            InitializeComponent();
            cbEncoding.SelectedIndex = 0;
            tcpServer.EventStateChanged += TcpServer_OnStateChanged;
            tcpServer.EventConnection += TcpServer_OnConnection;
            tcpServer.EventDisconnect += TcpServer_OnDisconnect;
            tcpServer.EventAfterReceive += TcpServer_OnAfterReceive;
        }

        private void TcpServer_OnStateChanged(object sender, StateEventArgs e)
        {
            BeginInvoke(() =>
            {
                switch (e.State)
                {
                    case EnumServerStateType.运行:
                        Print($"【服务已经启动】......{Environment.NewLine}");
                        Total = 0;
                        break;
                    case EnumServerStateType.停止:
                        Print($"【服务已经停止】{Environment.NewLine}");
                        break;
                }
                BtnStart.Enabled = e.State == EnumServerStateType.停止;
                BtnStop.Enabled = e.State != EnumServerStateType.停止;
            });
        }

        private int Total = 0;
        private void TcpServer_OnAfterReceive(object sender, DataEventAges e)
        {
            BeginInvoke(() =>
            {
                string strOut = (string)cbEncoding.SelectedItem switch
                {
                    "ASCII" => Encoding.ASCII.GetString(e.Data),
                    "UTF-8" => Encoding.UTF8.GetString(e.Data),
                    "HEX" => BitConverter.ToString(e.Data).Replace("-", " "),
                    _ => string.Empty,
                };
                var _ = ((Server)sender).SendAsync(e.IP_Port, e.Data);
                Print($"【{++Total}】.【{e.IPAddress}:{e.Port}】: {strOut}{Environment.NewLine}");
            });
        }

        private void TcpServer_OnDisconnect(object sender, OnlineClient e)
        {
            BeginInvoke(() =>
            {
                Print($"【{e.IP_Port}】下线{Environment.NewLine}");
                lbClient.Items.Remove(e.IP_Port);
            });
        }

        private void TcpServer_OnConnection(object sender, OnlineClient e)
        {
            BeginInvoke(() =>
            {
                Print($"【{e.IP_Port}】上线{Environment.NewLine}");
                lbClient.Items.Add(e.IP_Port);
            });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                tcpServer.IpAddress = IPAddress.Parse(txtIp.Text);
                tcpServer.Port = int.Parse(txtPort.Text);
                tcpServer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            tcpServer.Stop();
        }

        private void Print(string text)
        {
            BeginInvoke(() =>
            {
                rtxt.AppendText(text);
                rtxt.ScrollToCaret();
            });
        }
    }
}
