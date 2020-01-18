using System;
using System.Collections.Generic;
using System.Linq;
using Common.Messages;

namespace ServerTest
{
    class ChatMessageSender : IChatMessageSender
    {
        private readonly IEnumerable<ConnectedEndpoint> _endpoints;

        public ChatMessageSender(IEnumerable<ConnectedEndpoint> endpoints)
        {
            _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public void Send(ChatMessage chatMessage)
        {
            if (chatMessage == null) throw new ArgumentNullException(nameof(chatMessage));

            ConnectedEndpoint target =
                _endpoints.FirstOrDefault(x => x.IsAuthorized && x.Login == chatMessage.ReceiverLogin);
            target?.Send(chatMessage);
        }
    }
}