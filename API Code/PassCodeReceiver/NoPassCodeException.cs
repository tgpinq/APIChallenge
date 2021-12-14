using System;
using System.Runtime.Serialization;

namespace APIChallengeClassLibrary.PassCodeReceiver
{
    [Serializable]
    public class NoPassCodeException : Exception
    {
        public NoPassCodeException()
        {
        }

        public NoPassCodeException(string message) : base(message)
        {
        }

        public NoPassCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoPassCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}