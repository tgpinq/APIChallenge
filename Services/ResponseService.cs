using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIChallengeClassLibrary
{
	public class ResponseService
	{
		private ILogger _log;
        private ITableService _tableService;

        public ResponseService(ILogger log, ITableService tableService)
		{
			_log = log;
			_tableService = tableService;
		}

		public ObjectResult GenerateResponseWithPassword()
		{
            string password = _tableService.GetPassword();

			BodyWithSinglePasswordProperty body = new BodyWithSinglePasswordProperty() { Password = "Password goes here." };
            string message = $"Congratulations the password is {password}, you need to submit that to the 'GeneratePassCode' resource at this endpoint. An example of the body required for that URL is: {JsonConvert.SerializeObject(body)}";
            UsefulPasswordResponseObject responseObject = new UsefulPasswordResponseObject(password, message);
			
			return new OkObjectResult(JsonConvert.SerializeObject(responseObject)); 
		}

		public ObjectResult GenerateUnsuccessfulResponse(string message)
		{
			return new BadRequestObjectResult(message);
		}

		public ObjectResult CreateCorrectPasswordResponse(Guid passcode)
		{
			string message = $"Congratulations, you have provided the correct password, the passcode is {passcode}. This pass code should be POSTED to the 'PostPassCode' resource of this endpoint. The body will be the same but replacing the property Password with PassCode.";
			UsefulPassCodeResponse response = new UsefulPassCodeResponse(passcode, message);
			return new OkObjectResult(JsonConvert.SerializeObject(response));
		}

		public ObjectResult CreateIncorrectPasswordResponse()
		{
			return new BadRequestObjectResult($"The password you provided is not correct, or is incorrectly formatted, please try again.");
		}

		public ObjectResult ValidPassCodeResponse()
		{
			return new OkObjectResult("Congratulations, your passcode was correct. This marks the end of the first part of the exercise. \n \n Your next challenge is to write a script or small application in a language of your choice. \n This application when started should provide the password to the passcode endpoint which will return a passcode.\n Your application should then send that passcode to the passcode receiver endpoint and report back to the user whether or not the passcode was correct. \n This should be done with out any manual inputs (other than starting the program). \n \n Essentially you are automating the manual process you just completed through Postman now that you know what values are needed.\n \n This may seem daunting but break it down into small parts. The first step is simply sending a request to the endpoint. Google how to send an http request in the language you want to use. In most cases it will be one or two lines of code. \n Having done that the next step is to send it with the location parameter in the URL, exactly as you have done in postman. \n When you can do that you can then look at how to add the body. Google is likely to be your friend here. \n Once you can send the body and are getting a 2XX response back you can look at how to read the return data. \n from there the last bit is pulling out the password value. At this point the rest should be straight forward.  \n \nIf you need help or more information please make sure you ask. ");
		}

		public ObjectResult InvalidPassCodeResponse()
		{
			return new BadRequestObjectResult("Your pass code was not valid. Please try again. Please note that pass codes may only be used once.");
		}

        public ObjectResult GenerateDeserialiseErrorResponse(string message)
        {
			return new BadRequestObjectResult($"Your submitted JSON could not be deserialised correctly, there is likely a format or structure error in your json. \n The website 'https://jsonlint.com/?code=' will help you work out if your json is valid or not. \n The actual error thrown during the attempt to deserialise was: '{message}'.");
        }

        public ObjectResult GenerateUnexpectedExceptionThrownResponse(string message)
        {
			return new BadRequestObjectResult($"You managed to break the program somehow, congratulations, you are a proper user! Please make Tom aware so that he can work out what has gone wrong. The actual exception message that was thrown was: '{message}'.");
        }

        public ObjectResult GenerateValidPasswordUpdateResponse()
        {
			return new OkObjectResult("Password Updated Successfully");
        }
    }
}
