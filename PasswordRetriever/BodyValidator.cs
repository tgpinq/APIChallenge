using System;

namespace APIChallengeClassLibrary
{
	public class BodyValidator
	{		
		public void ValidateBody(APIBody body)
		{
			if (body == null)
            {
				throw new Exception("No body provided with your request, please provide a body. This body should be made up of Json");
            }
			//Capital City
			if (string.IsNullOrEmpty(body.CapitalCity))
			{
				throw new Exception("Body does not contain a required string value for property 'CapitalCity'. The value should be the capital city of Azerbaijan");
			}
			if (body.CapitalCity.ToLower() != "baku")
			{
				throw new Exception("Value provided as the capital city of Azerbaijan is not correct. Please ensure that it is a string has four characters and starts with b");
			}
			//List Of Random Strings

			if (body.ListOfRandomStrings == null)
			{
				throw new Exception("You have not provided a collection of strings for the property 'ListOfRandomStrings' please provide a collection of strings");
			}
			if (body.ListOfRandomStrings.Count != 4)
			{
				throw new Exception("Your collection of random strings for property 'ListOfRandomStrings' does not have exactly 4 strings, please ensure it has 4 strings.");
			}
			foreach (string randomString in body.ListOfRandomStrings)
			{
				if (randomString.Contains("a")||randomString.Contains("e") || randomString.Contains("i") || randomString.Contains("o") || randomString.Contains("u"))
				{
					throw new Exception($"One of your strings in the collection ListOfRandomStrings contains a lower case vowel. Please ensure none of the strings contain a lower case vowel.");
				}
			}

			//TomsAgeWhenHeWroteThisCode
			if (body.Age == 0)
			{
				throw new Exception("Your body must contain an 'Age' Property. The value of this property must be Tom's age in years when he wrote this code.");
			}
			if (body.Age < 36)
			{
				throw new Exception("Tom was older than that, try again.");
			}
			if(body.Age > 36)
			{
				throw new Exception("Tom was younger that that, try again.");
			}
		}
	}
}