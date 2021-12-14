using System;

namespace APIChallengeClassLibrary
{
    internal class UsefulPassCodeResponse
    {
        public UsefulPassCodeResponse(Guid passcode, string message)
        {
            Passcode = passcode;
            Message = message;
        }

        public Guid Passcode { get; }
        public string Message { get; }
    }
}