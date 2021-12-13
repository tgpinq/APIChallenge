using System;
using System.Runtime.Serialization;

namespace APIChallengeClassLibrary
{
    [Serializable]
    public class NoBodyException : Exception
    {
        public NoBodyException()
        {
        }

        public NoBodyException(string message) : base(message)
        {
        }

        public NoBodyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoBodyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}