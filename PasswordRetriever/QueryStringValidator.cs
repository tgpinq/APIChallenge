using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIChallengeClassLibrary
{
	public class QueryStringValidator
	{
		private string _queryInstructions = "please submit a query with the key 'location' and a value of the city in which Tom works.";

		public void ValidateQueryString(Dictionary<string, StringValues> queryStringKeyValuePairs)
		{
			if (queryStringKeyValuePairs.Count == 0)
			{
				throw new Exception($"You have not submitted a query in the URL, {_queryInstructions}");
			}

			StringValues location;

			if (queryStringKeyValuePairs.TryGetValue("location", out location) == false)
			{
				throw new Exception($"You did not submit a location key in the URL query, {_queryInstructions} ");
			}
			else
			{
				if (location.ToString().ToLower() != "southampton")
				{
					throw new Exception("Sorry you did not provide the correct city in which Tom works, remember he works at the Ordnance Survey Head Office.");
				}
			}
		}
	}
}
