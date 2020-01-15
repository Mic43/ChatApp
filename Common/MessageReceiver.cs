using System;
using Common.Interfaces;
using Common.Messages;

namespace Common
{
    public class MessageReceiver<TMessage>  where TMessage:IMessage
    {
        private IMessageReceiver _messageReceiver;

        public MessageReceiver(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
        }

        public TMessage Receive()
        {
            return (TMessage) _messageReceiver.Receive();
        }

    }
}