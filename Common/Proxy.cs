using System;
using System.IO;
using System.Net.Sockets;
using Common.Interfaces;
using Common.Messages;

namespace Common
{
    public class Proxy :IDisposable
    {
        public string HostIp { get; }
        public int HostPort { get; }

        private TcpClient _client;
        private BinaryMessageProcessor _binaryMessageProcessor;
        private IMessageSender _messageSender;
        private IMessageReceiver _messageReceiver;
        private bool IsConnected => _client != null && _client.Connected;

        public Proxy(string hostIp,int hostPort)
        {
            HostIp = hostIp ?? throw new ArgumentNullException(nameof(hostIp));
            HostPort = hostPort;
        }

        public void Connect()
        {
            _client = new TcpClient(HostIp, HostPort);

            _binaryMessageProcessor = new BinaryMessageProcessor();

            _messageSender = new TcpMessageSender(_client, _binaryMessageProcessor);
            _messageReceiver = new TcpMessageReceiver(_client, _binaryMessageProcessor);
        }
        public (bool IsSuccess, Exception error) TryConnect()
        {
            try
            {
                Connect();
                return (true, null);
            }
            catch (SocketException e)
            {
                return (false, e);
            }
        }

        public void Call<TRequest>(TRequest request)
            where TRequest : IMessage 
        {
            if (!IsConnected)
                throw new InvalidOperationException("Proxy is not connected, call Connect function first");

            _messageSender.Send(request);
        }
        public (bool IsSuccess, Exception error) TryCall<TRequest>(TRequest request)
            where TRequest : class, IMessage
        {
            try
            {
                Call(request);
                return (true, null);

            }
            catch (IOException e)
            {
                return (
false, e);
            }
        }

        public TResponse Call<TRequest, TResponse>(TRequest request)
            where TRequest : IMessage where TResponse : IMessage
        {
            if (!IsConnected)
                throw new InvalidOperationException("Proxy is not connected, call Connect function first");

            var receiver = new MessageReceiver<TResponse>(_messageReceiver);

            _messageSender.Send(request);
            return receiver.Receive();
        }
        public (TResponse Response,bool IsSuccess,Exception error) TryCall<TRequest, TResponse>(TRequest request)
            where TRequest : class,IMessage where TResponse : class,IMessage
        {
            try
            {
                var response = Call<TRequest, TResponse>(request);
                return (response, true, null);
            }
            catch (IOException e)
            {
                return (null, false, e);
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}