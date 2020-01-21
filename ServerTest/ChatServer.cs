using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ServerTest
{
    class ChatServer
    {
        private object _locker = new object();
        private readonly ILogger _logger;

        private readonly string _host;
        private readonly int _port;
        private readonly ConcurrentDictionary<Guid,ConnectedEndpoint> _clients 
            = new ConcurrentDictionary<Guid, ConnectedEndpoint>();
        private readonly TcpListener _serverSocket;
        private bool _stopServer = false;
        private readonly BinaryMessageProcessor _binaryMessageProcessor = new BinaryMessageProcessor();

        public ChatServer(string host, int port, ILogger logger)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _port = port;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _serverSocket = new TcpListener(IPAddress.Parse(host), port);
        }

        public bool IsStopped
        {
            get
            {
                lock (_locker)
                {
                    return _stopServer;
                }
            }
            private set
            {
                lock (_locker)
                {
                    _stopServer = value;
                }
            }
        }

        public void Start()
        {
            List<Task> tasks = new List<Task>();
            _serverSocket.Start();
            IChatMessageSender messageSender = new ChatMessageSender(_clients.AsEnumerable().Select(x=>x.Value));
            while (!IsStopped)
            {
                Thread.Sleep(1000);
                if (_serverSocket.Pending())
                {
                    _logger.WriteLine("Client connected");
                    TcpClient clientSocket = _serverSocket.AcceptTcpClient();

                    var connectedEndpoint = 
                        new ConnectedEndpoint(Guid.NewGuid(),clientSocket, _binaryMessageProcessor,
                            _binaryMessageProcessor, _logger, messageSender);

                    void OnDisconnected(object sender, EventArgs args)
                    {
                        _clients.Remove(((ConnectedEndpoint) sender).Id,out connectedEndpoint);
                    }
                    connectedEndpoint.OnDisconnected += OnDisconnected;

                    _clients.TryAdd(connectedEndpoint.Id,connectedEndpoint);

                    tasks.Add(Task.Run(() => connectedEndpoint.Handle()));
                }
            }

            foreach (var connectedEndpoint in _clients.Values)
            {
                connectedEndpoint.Close();
            }

            Task.WaitAll(tasks.ToArray());
        }

        public void Stop()
        {
            _logger.WriteLine("Stopping...");
            IsStopped = true;
        }
    }
}