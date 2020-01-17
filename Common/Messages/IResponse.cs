using System;

namespace Common.Messages
{
    [Serializable]
    public class Response : IMessage
    {
        public bool IsSuccess { get; set; }

        public Response(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}