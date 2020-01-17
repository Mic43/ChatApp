using System;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Common.Messages;

namespace ServerTest
{
    class ConnectedEndpoint
    {
        private readonly TcpClient _clientSocket;
        private readonly IMessageDeserializer _deserializer;
        private readonly IMessageSerializer _messageSerializer;
        private TcpMessageSender _sender;

        public bool IsAuthorized => AuthorizationToken != null;
        public string AuthorizationToken { get; set; } = null;

        public ConnectedEndpoint(TcpClient clientSocket, IMessageDeserializer deserializer,
            IMessageSerializer messageSerializer)
        {
            _clientSocket = clientSocket ?? throw new ArgumentNullException(nameof(clientSocket));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            _sender = new TcpMessageSender(_clientSocket, _messageSerializer);
        }

        private bool CheckAuthorization(AuthorizedMessage authorizedMessage)
        {
            return IsAuthorized && AuthorizationToken == authorizedMessage.SenderAuthorizationToken;
        }
        private void Handle(LoginRequest loginRequest) {
            AuthorizationToken = Guid.NewGuid().ToString();
            _sender.Send(new LoginResponse(true, AuthorizationToken));
        }
        private void Handle(ChatMessage chatMessage)
        {
            if (!CheckAuthorization(chatMessage))
            {
                Console.WriteLine("Not authorized");
                _sender.Send(new Response(false));
                return;
            }

            Console.WriteLine(chatMessage.Text);
            _sender.Send(new Response(true));

        }

        public void Handle()
        {
            while (_clientSocket.Connected)
            {
                dynamic message = new TcpMessageReceiver(_clientSocket, _deserializer).Receive();
                Handle(message);
            }
        }

        public void Close()
        {
            _clientSocket.Close();
        }
    }
}