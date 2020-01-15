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
        public MainFrm()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 14001))
            {
                var binaryMessageProcessor = new BinaryMessageProcessor();

                IMessageSender messageSender = new TcpMessageSender(client, binaryMessageProcessor);
                IMessageReceiver messageReceiver = new TcpMessageReceiver(client, binaryMessageProcessor);

                var receiver = new MessageReceiver<LoginResponse>(messageReceiver);

                messageSender.Send(new LoginRequest("admin", "haslo"));
                LoginResponse response = receiver.Receive();
                richTextBox1.AppendText(response.ToString());

                client.Close();
            }

        }
    }
}
