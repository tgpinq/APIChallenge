using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace APIChallengeClassLibrary
{
	public class PasswordRetrieverOrchestrator
	{
		private QueryStringValidator _queryStringValidator;
		private ILogger _log;
		private BodyValidator _bodyValidator;

		public PasswordRetrieverOrchestrator(ILogger log, QueryStringValidator queryStringValidator, BodyValidator bodyValidator)
		{
			_queryStringValidator = queryStringValidator;
			_log = log;
			_bodyValidator = bodyValidator;
		}
		public void ProcessRequest(HttpRequest request)
		{
			ProcessRequestQueryString(request.Query);
			ProcessRequestBody(request.Body);
		}

		private void ProcessRequestQueryString(IQueryCollection query)
		{
			Dictionary<string, StringValues> queryStringKeyValuePairs = new Dictionary<string, StringValues>();
			foreach (var item in query)
			{
				queryStringKeyValuePairs.Add(item.Key, item.Value);
			}
			_queryStringValidator.ValidateQueryString(queryStringKeyValuePairs);
		}

		private void ProcessRequestBody(Stream receivedBody)
		{
			string requestBody = new StreamReader(receivedBody).ReadToEndAsync().Result;
			
			APIBody body = JsonConvert.DeserializeObject<APIBody>(requestBody);

			_bodyValidator.ValidateBody(body);
			
			
		}

	}
}
