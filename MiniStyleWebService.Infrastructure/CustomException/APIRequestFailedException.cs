using System;

namespace MiniStyleWebService.Infrastructure.CustomException
{
    [Serializable]
    public class APIRequestFailedException : Exception
    {
        public APIRequestFailedException(string message)
            : base(message) { }
    }
}
