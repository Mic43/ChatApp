using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageDeserializer
    {
        IMessage Deserialize(BinaryData message);
    }
}