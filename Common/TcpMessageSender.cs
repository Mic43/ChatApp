using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Common.Interfaces;
using Common.Messages;

namespace Common
{
    public class TcpMessageSender : IMessageSender
    {
        private readonly TcpClient _tcpClient;
        private readonly IMessageSerializer _messageSerializer;

        public TcpMessageSender(TcpClient tcpClient,IMessageSerializer messageSerializer)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }
        public void Send(IMessage message)
        {
            using (var binaryWriter = new BinaryWriter(_tcpClient.GetStream(),Encoding.UTF8,true))
            {
                var buffer = _messageSerializer.Serialize(message).Bytes;
                binaryWriter.Write(buffer.Length);
                binaryWriter.Write(buffer);
            }
        }
    }
}