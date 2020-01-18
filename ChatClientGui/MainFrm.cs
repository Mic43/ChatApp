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
        private Proxy _serverProxy;
        private string _responseNewAuthorizationToken;
        private string _loggedUserLogin;

        public MainFrm()
        {
            InitializeComponent();
            _serverProxy = new Proxy("127.0.0.1", 14002);
        }

        private void Connect()
        {
            var result = _serverProxy.TryConnect();
            if (!result.IsSuccess)
            {
                MessageBox.Show($"Error: {result.error}");
                Application.Exit();
            }
        }
        private void TryLogin()
        {
            _loggedUserLogin = Microsoft.VisualBasic.Interaction.InputBox("UserName", "User name");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Password", "Password");
          
            var result = _serverProxy.TryCall<LoginRequest, LoginResponse>(new LoginRequest(_loggedUserLogin, password));
            if (!result.IsSuccess)
            {
                MessageBox.Show($"Error: {result.error}");
                Application.Exit();
            }
            else 
            {
                if (result.Response.IsSuccess)
                    _responseNewAuthorizationToken = result.Response.NewAuthorizationToken;
                else
                {
                    MessageBox.Show("Login unsuccesful");
                    Application.Exit();
                }
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            Connect();
            TryLogin();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            _serverProxy.Call(
                new ChatMessage(_responseNewAuthorizationToken,
                    richTextBoxInput.Text, _loggedUserLogin, textBoxReceiver.Text));
        }
    }
}