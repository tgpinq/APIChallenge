using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using APIChallengeClassLibrary;
using Microsoft.Extensions.Logging;
using APIChallengeClassLibrary.PassCodeReceiver;
using APIChallengeClassLibrary.PasswordOrchestrator;

namespace APIChallenge
{
    public class Startup : IWebJobsStartup
	{
		public void Configure(IWebJobsBuilder builder)
		{

			IConfiguration config = (IConfiguration)builder.Services.BuildServiceProvider().GetService(typeof(IConfiguration));

			string connectionString = config["StorageAccountConnectionString"];
			string tableName = config["TableName"];			
			string password = config["Password"];
			string passcodePartitionKey = config["PasscodePartitionKey"];
			string passwordPartitionKey = config["PasswordPartitionKey"];
			
			LoggerFactory logFactory = new LoggerFactory();
			ILogger log = logFactory.CreateLogger("APIChallenge");

			TableClient tableClient = new TableClient(connectionString, tableName);
			QueryStringValidator queryStringValidator = new QueryStringValidator();
			BodyValidator bodyValidator = new BodyValidator();
			TableService tableService = new TableService(tableClient, passcodePartitionKey, passwordPartitionKey, password);

			builder.Services.AddScoped(responseService => new ResponseService(log,tableService));			
			builder.Services.AddScoped(passwordOrch => new PasswordRetrieverOrchestrator(log, queryStringValidator, bodyValidator));
			builder.Services.AddScoped(pcg => new PassCodeGenerator(tableService));
			builder.Services.AddScoped(pcr => new PassCodeReceiver(tableService));
			builder.Services.AddScoped(pwo => new PasswordOrchestrator(passwordPartitionKey,tableService));


		}
	}


}
