using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
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
            using (BinaryReader reader = new BinaryReader(_tcpClient.GetStream(), Encoding.UTF8, true))
            {
                try
                {
                    int dataSize = reader.ReadInt32();
                    var readBytes = reader.ReadBytes(dataSize);
                    if (readBytes.Length != dataSize)
                        throw new MessageReadException($"Wrong size of received data. Expected: {dataSize} Received: {readBytes}");
                    return _messageDeserializer.Deserialize(new BinaryData(readBytes));
                }
                catch (IOException)
                {
                    throw;
                }
            }
        }
    }
}