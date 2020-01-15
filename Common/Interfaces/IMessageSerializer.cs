using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageSerializer
    {
        BinaryData Serialize(IMessage message);
    }
}