using System;

namespace Common
{
    public class MessageReadException : Exception
    {
        public MessageReadException(string message)  : base(message)
        {
            
        }
    }
}