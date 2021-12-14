using System;
using System.Runtime.Serialization;

namespace APIChallengeClassLibrary
{
	[Serializable]
	public class InvalidPassWordException : Exception
	{
		public InvalidPassWordException()
		{
		}

		public InvalidPassWordException(string message) : base(message)
		{
		}

		public InvalidPassWordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidPassWordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}