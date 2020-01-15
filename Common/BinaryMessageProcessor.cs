using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Interfaces;
using Common.Messages;

namespace Common
{
    public class BinaryMessageProcessor : IMessageSerializer, IMessageDeserializer
    {
        public BinaryData Serialize(IMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, message);
                return new BinaryData(memoryStream.ToArray());
            }
        }

        public IMessage Deserialize(BinaryData message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            using (var memoryStream = new MemoryStream(message.Bytes))
            {
                object deserialized = new BinaryFormatter().Deserialize(memoryStream);

                if (!(deserialized is IMessage))
                    throw new InvalidOperationException("message is not IMessageType");
                

                return (IMessage)deserialized;
            }
        }
    }
}