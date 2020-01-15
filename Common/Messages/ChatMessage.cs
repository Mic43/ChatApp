using System;

namespace Common.Messages
{
    [Serializable]
    public class ChatMessage : AuthorizedMessage
    {
        public ChatMessage(string senderAuthorizationToken, string text, string senderLogin, string receiverLogin) : base(senderAuthorizationToken)
        {
            Text = text;
            SenderLogin = senderLogin;
            ReceiverLogin = receiverLogin;
        }

        public string Text { get; set; }
        public string SenderLogin { get; set; }
        public string ReceiverLogin { get; set; }
    }
}