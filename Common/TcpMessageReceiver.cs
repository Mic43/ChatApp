using System;
using System.IO;
using System.Net.Sockets;
using Common.Interfaces;
using Common.Messages;

namespace Common
{
    public class TcpMessageReceiver : IMessageReceiver
    {
        private readonly TcpClient _tcpClient;
        private readonly IMessageDeserializer _messageDeserializer;

        public TcpMessageReceiver(TcpClient tcpClient, IMessageDeserializer messageDeserializer)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _messageDeserializer = messageDeserializer ?? throw new ArgumentNullException(nameof(messageDeserializer));
        }

        public IMessage Receive()
        {
            using (BinaryReader reader = new BinaryReader(_tcpClient.GetStream()))
            {
                try
                {
                    return _messageDeserializer.Deserialize(new BinaryData(reader.ReadFully()));
                }
                catch (IOException)
                {
                    throw;
                }
            }
        }
    }
}