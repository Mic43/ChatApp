namespace Common.Messages
{
    public abstract class AuthorizedMessage:IMessage
    {
        public string SenderAuthorizationToken { get; set; }

        protected AuthorizedMessage(string senderAuthorizationToken)
        {
            SenderAuthorizationToken = senderAuthorizationToken;
        }
    }
}