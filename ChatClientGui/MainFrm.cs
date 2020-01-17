using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Common.Interfaces;
using Common.Messages;

namespace ChatClientGui
{
    public partial class MainFrm : Form
    {
        private  Proxy _serverProxy;
        private string _responseNewAuthorizationToken;

        public MainFrm()
        {
            InitializeComponent();
            _serverProxy = new Proxy("127.0.0.1", 14002);
        }

        private void Connect()
        {
            var result = _serverProxy.TryConnect();
            if (!result.IsSuccess)
                MessageBox.Show($"Error: {result.error}");

        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var result = _serverProxy.TryCall<LoginRequest, LoginResponse>(new LoginRequest("admin", "haslo"));
            if (!result.IsSuccess)
                MessageBox.Show($"Error: {result.error}");
            else
            {
                _responseNewAuthorizationToken = result.Response.NewAuthorizationToken;
                richTextBox1.AppendText(result.Response.ToString());
            }

//            using (TcpClient client = new TcpClient("127.0.0.1", 14001))
//            {
//                var binaryMessageProcessor = new BinaryMessageProcessor();
//
//                IMessageSender messageSender = new TcpMessageSender(client, binaryMessageProcessor);
//                IMessageReceiver messageReceiver = new TcpMessageReceiver(client, binaryMessageProcessor);
//
//                var receiver = new MessageReceiver<LoginResponse>(messageReceiver);
//
//                messageSender.Send(new LoginRequest("admin", "haslo"));
//                LoginResponse response = receiver.Receive();
//                richTextBox1.AppendText(response.ToString());
//
//                client.Close();
//            }

        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            Connect();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            _serverProxy.Call<ChatMessage, Response>(
                new ChatMessage(_responseNewAuthorizationToken,
                    richTextBoxInput.Text, "", ""));
        }
    }
}
