using System;

namespace Common.Messages
{
    [Serializable]
    public class LoginResponse : IMessage
    {
        public bool IsSuccess { get; set; }
        public string NewAuthorizationToken { get; set; }

        public LoginResponse(
            bool isSuccess, 
            string newAuthorizationToken) 
        {
            IsSuccess = isSuccess;
            NewAuthorizationToken = newAuthorizationToken ?? throw new ArgumentNullException(nameof(newAuthorizationToken));
        }

        public override string ToString()
        {
            return $"IsSuccess: {IsSuccess}, NewAuthorizationToken: {NewAuthorizationToken}";
        }
    }
}