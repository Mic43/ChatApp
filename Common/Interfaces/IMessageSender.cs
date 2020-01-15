using System;
using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageSender
    {
        void Send(IMessage message);
    }
}