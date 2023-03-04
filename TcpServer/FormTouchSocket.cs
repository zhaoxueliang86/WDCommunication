using System.Data;
using System.Net;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using WDCommunication.WDTcp;

namespace TcpServer
{
    public partial class FormTouchSocket : Form
    {
        private readonly TcpService service = new();
        private int Total = 0;
        public FormTouchSocket()
        {
            InitializeComponent();
            cbEncoding.SelectedIndex = 0;
            service.Connecting = (client, e) => //有客户端正在连接
            {
            };

            service.Connected = (client, e) => //有客户端成功连接
            {
                BeginInvoke(() =>
                {
                    Print($"【{client.GetIPPort()}】上线{Environment.NewLine}");
                    lbClient.Items.Add(client.GetIPPort());
                });
            };

            service.Disconnected = (client, e) =>//有客户端断开连接
            {
                BeginInvoke(() =>
                {
                    Print($"【{client.GetIPPort()}】下线{Environment.NewLine}");
                    lbClient.Items.Remove(client.GetIPPort());
                });
            };
            service.Received = (client, byteBlock, requestInfo) =>
            {
                //从客户端收到信息
                byte[] data = new byte[byteBlock.Len];
                data = byteBlock.Buffer.Take(data.Length).ToArray();
                string mes = Encoding.UTF8.GetString(data);

                client.Logger.Info($"已从{client.ID}接收到信息：{mes}");

                BeginInvoke(() =>
                {
                    string strOut = (string)cbEncoding.SelectedItem switch
                    {
                        "ASCII" => Encoding.ASCII.GetString(data),
                        "UTF-8" => Encoding.UTF8.GetString(data),
                        "HEX" => BitConverter.ToString(data).Replace("-", " "),
                        _ => string.Empty,
                    };
                    Print($"【{++Total}】.【{client.GetIPPort()}】: {strOut}{Environment.NewLine}");
                });
            };
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            service.Setup(new TouchSocketConfig()//载入配置     
                .SetListenIPHosts(new IPHost[] { new IPHost($"tcp://{txtIp.Text}:{txtPort.Text}") })//同时监听两个地址
                .ConfigureContainer(a =>//容器的配置顺序应该在最前面
                {
                    a.AddConsoleLogger();//添加一个控制台日志注入（注意：在maui中控制台日志不可用）
                })
                .ConfigurePlugins(a =>
                {
                    //a.Add();//此处可以添加插件
                }))
                .Start();//启动

            Print($"【服务已经启动】......{Environment.NewLine}");
            BtnStart.Enabled = false;
            BtnStop.Enabled = !BtnStart.Enabled;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            Total = 0;
            service.Stop();
            Print($"【服务已经停止】{Environment.NewLine}");
            BtnStart.Enabled = true;
            BtnStop.Enabled = !BtnStart.Enabled;
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
