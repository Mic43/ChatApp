using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace ServerTest
{
    class ChatServer
    {
        private object _locker = new object();

        private readonly string _host;
        private readonly int _port;
        private readonly ConcurrentBag<ConnectedEndpoint> _clients = new ConcurrentBag<ConnectedEndpoint>();
        private readonly TcpListener _serverSocket;
        private bool _stopServer = false;
        private BinaryMessageProcessor _binaryMessageProcessor;

        public ChatServer(string host,int port)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _port = port;

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
            while (!IsStopped)
            {
                Thread.Sleep(2000);
                if (_serverSocket.Pending())
                {
                    TcpClient clientSocket = _serverSocket.AcceptTcpClient();
                    _binaryMessageProcessor = new BinaryMessageProcessor();
                    var connectedEndpoint = new ConnectedEndpoint(clientSocket,_binaryMessageProcessor,_binaryMessageProcessor);
                    _clients.Add(connectedEndpoint);
                    tasks.Add(Task.Run(() => connectedEndpoint.Handle()));
                }
            }
            foreach (var connectedEndpoint in _clients)
            {
                connectedEndpoint.Close();
            }
            Task.WaitAll(tasks.ToArray());
        }

        public void Stop()
        {
            IsStopped = true;
        }

    }
}