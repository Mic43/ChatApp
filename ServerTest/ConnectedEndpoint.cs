using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Common.Messages;
using ServerTest.Db;

namespace ServerTest
{
    class ConnectedEndpoint
    {
        public Guid Id { get; }
        private readonly TcpClient _clientSocket;
        private readonly IMessageDeserializer _deserializer;
        private readonly IMessageSerializer _messageSerializer;
        private readonly ILogger _logger;
        private readonly IChatMessageSender _chatMessageSender;
        private readonly TcpMessageSender _sender;
        
        public bool IsAuthorized => AuthorizationToken != null;
        public string AuthorizationToken { get; private set; } = null;
        public string Login { get; private set; }

        public event EventHandler<EventArgs> OnDisconnected;


        public ConnectedEndpoint(Guid id, TcpClient clientSocket, IMessageDeserializer deserializer,
            IMessageSerializer messageSerializer, ILogger logger, IChatMessageSender chatMessageSender)
        {
            Id = id;
            _clientSocket = clientSocket ?? throw new ArgumentNullException(nameof(clientSocket));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chatMessageSender = chatMessageSender;
            _sender = new TcpMessageSender(_clientSocket, _messageSerializer);
        }

        private bool CheckAuthorization(AuthorizedMessage authorizedMessage)
        {
            return IsAuthorized && AuthorizationToken == authorizedMessage.SenderAuthorizationToken;
        }
        private void Handle(LoginRequest loginRequest)
        {
            User user;
            using (var dbContext = new UsersDataContext())
            {
                 user = dbContext.Users.Find(loginRequest.Login);
            }

            if (user == null || user.Password != loginRequest.Password)
            {
                _sender.Send(new LoginResponse(false, null));
                return;
            }

            AuthorizationToken = Guid.NewGuid().ToString();
            Login = loginRequest.Login;
            _logger.WriteLine($"User: {user.Login} logged.");

            _sender.Send(new LoginResponse(true, AuthorizationToken));
        }
        private void Handle(ChatMessage chatMessage)
        {
            if (!CheckAuthorization(chatMessage))
            {
                _logger.WriteLine("Not authorized");
                return;
            }

            _logger.WriteLine($"{chatMessage.SenderLogin} to " +
                              $"{chatMessage.ReceiverLogin}: {chatMessage.Text}");
            _chatMessageSender.Send(chatMessage);
        }

        public void Handle()
        {
            while (_clientSocket.Connected)
            {
                try
                {
                    dynamic message = new TcpMessageReceiver(_clientSocket, _deserializer).Receive();
                    Handle(message);
                }
                catch (EndOfStreamException e)
                {
                    _logger.WriteLine("Client disconnected");
                    OnDisconnected?.Invoke(this,EventArgs.Empty);
                    Close();
                }
                catch (IOException e)
                {
                    _logger.WriteLine("ERROR:" + e);
                }
            }
        }
        public void Send(IMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _sender.Send(message);
        }
        public void Close()
        {
            _clientSocket.Close();
        }
    }
}