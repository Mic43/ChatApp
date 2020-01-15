using System;

namespace Common.Messages
{
    [Serializable]
    public class LoginRequest : IMessage
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public LoginRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return $"Login: {Login}, Password: {Password}";
        }
    }
}