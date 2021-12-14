namespace APIChallengeClassLibrary
{
    public class UsefulPasswordResponseObject
    {
        public string Password { get; }
        public string Message { get; }

        public UsefulPasswordResponseObject(string password, string message)
        {
            Password = password;
            Message = message;
        }

    }
}