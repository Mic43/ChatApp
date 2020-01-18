using System;

namespace Common.Messages
{
    [Serializable]
    public class LoginResponse : Response
    {
        public string NewAuthorizationToken { get; set; }

        public LoginResponse(
            bool isSuccess, 
            string newAuthorizationToken) : base(isSuccess)
        {
            NewAuthorizationToken = newAuthorizationToken;
        }

        public override string ToString()
        {
            return $"IsSuccess: {IsSuccess}, NewAuthorizationToken: {NewAuthorizationToken}";
        }
    }
}