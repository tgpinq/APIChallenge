using System;
using System.Runtime.Serialization;

namespace APIChallengeClassLibrary
{
    [Serializable]
    public class NoPassWordException : Exception
    {
        public NoPassWordException()
        {
        }

        public NoPassWordException(string message) : base(message)
        {
        }

        public NoPassWordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoPassWordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}