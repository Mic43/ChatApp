using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageReceiver
    {
        IMessage Receive();
    }
}