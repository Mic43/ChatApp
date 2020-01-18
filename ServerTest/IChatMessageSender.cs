using System.Collections;
using Common.Messages;

namespace ServerTest
{
    internal interface IChatMessageSender
    {
        void Send(ChatMessage chatMessage);
    }
}