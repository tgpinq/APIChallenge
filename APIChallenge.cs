using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using APIChallengeClassLibrary;
using APIChallengeClassLibrary.PassCodeReceiver;
using Microsoft.Azure.WebJobs.Hosting;
using APIChallenge;
using APIChallengeClassLibrary.PasswordOrchestrator;

[assembly: WebJobsStartup(typeof(Startup))]
namespace APIChallenge
{

    public class APIChallenge
    {
        private ResponseService _responseService;
        private PasswordRetrieverOrchestrator _passwordRetrieverOrchestrator;
        private PassCodeGenerator _passCodeGenerator;
        private PassCodeReceiver _passCodeReceiver;
        private PasswordOrchestrator _passwordOrchestrator;

        public APIChallenge(ResponseService responseService, PasswordRetrieverOrchestrator prOrchestrator, PassCodeGenerator pcGenerator, PassCodeReceiver pcReceiver, PasswordOrchestrator pwOrchestrator)
        {
            _responseService = responseService;
            _passwordRetrieverOrchestrator = prOrchestrator;
            _passCodeGenerator = pcGenerator;
            _passCodeReceiver = pcReceiver;
            _passwordOrchestrator = pwOrchestrator;

        }
       
        [FunctionName("GetPassword")]
        public async Task<IActionResult> GetPassword(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("Triggered GetSecretURL Function");            
            ObjectResult result;

            try
			{
                _passwordRetrieverOrchestrator.ProcessRequest(req);
                result = _responseService.GenerateResponseWithPassword();
			}
			catch (Exception e)
			{
                if (e.Source.ToLower().Contains("newtonsoft.json"))
                {
                    result = _responseService.GenerateDeserialiseErrorResponse(e.Message);
                }
                else
                {
                    result = _responseService.GenerateUnsuccessfulResponse(e.Message);
                }
			}

            return result;
        }

        [FunctionName("GeneratePassCode")]
        public async Task<IActionResult> GeneratePassCode(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            			
            ObjectResult result;          

			try
			{
                Guid passcode = _passCodeGenerator.GeneratePassCodeAndAddToTable(req.Body);
                result = _responseService.CreateCorrectPasswordResponse(passcode);
			}
			catch (InvalidPassWordException e)
			{
                result = _responseService.CreateIncorrectPasswordResponse();				
			}
            catch(NoBodyException e)
            {
                result = _responseService.GenerateUnsuccessfulResponse(e.Message);
            }
            catch(NoPassWordException e)
            {
                result = _responseService.GenerateUnsuccessfulResponse(e.Message);
            }
            catch(Exception e)
			{
                result = _responseService.GenerateUnexpectedExceptionThrownResponse(e.Message);
			}
                   
            return result;
        }

        [FunctionName("PostPassCode")]
        public async Task<IActionResult> PostPassCode(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            ObjectResult result;
            
            try
            {
                if (_passCodeReceiver.PassCodeIsValid(req.Body))
			    {
                    result = _responseService.ValidPassCodeResponse();
			    }
			    else
			    {
                    result = _responseService.InvalidPassCodeResponse();
			    }
            }
            catch (Exception e)
            {
                result = _responseService.GenerateUnsuccessfulResponse($"I can't believe you got an exception at the final step! Potentially no helpful error message for you!! Take a look at the message thrown and see if you can work out what is wrong. If you can't figure it out, ask for help.The error message thrown was: {e.Message}");
            }
            return result;
        }

        [FunctionName("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            ObjectResult result = null;

            try
            {
                _passwordOrchestrator.UpdatePassword(req.Body);
                result = _responseService.GenerateValidPasswordUpdateResponse();
            }
            catch (Exception e)
            {

               result = _responseService.GenerateUnsuccessfulResponse($"Exception thrown whilst attempting to update password. Exception message is: {e.Message}");
            }
                   

            return result;
        }
    }	
}
            //Things to do:

            //Review TableService and how we are dealing with the two different partition keys. 

            //Things we could then do:

            // Write tests for pipeline to confirm that functions are up and working. e.g. get 200 back, get expected results back etc. Would require dev and prod instance deployments.
            // Add local function tests. - https://microsoft.github.io/AzureTipsAndTricks/blog/tip196.html need to check if works for 3.1. Also look at: https://docs.microsoft.com/en-us/azure/azure-functions/functions-test-a-function(may need to change functions back to static).
            // Add UpdateStatusEndPoint which will save student current status to table which can then be read from to display on a dashboard.

            // Add logging
            // Add some Easter eggs.
            // Deploy version to OS Environment with App Keys.
