using APIChallengeClassLibrary;
using Azure.Data.Tables;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
	public class ResponseServiceTests
	{
		private ResponseService _responseService;		

		public ResponseServiceTests()
		{
			LoggerFactory logFactory = new LoggerFactory();
			ILogger log = logFactory.CreateLogger("ResponseServiceTests");
			_responseService = new ResponseService(log, new TableService(new TestHelper().GetTableClient("testTable25"),"pk123","passwordPartitionKey","randomPassword"));
		}

		[Fact]
		public void ShouldReturnObjectResultWithPasswordWhenSuccessfulResponseIsCalled()
		{
			//Arrange

			

			//Act

			ObjectResult result = _responseService.GenerateResponseWithPassword();

			//Assert

			result.Value.ToString().Should().Contain("password");

		}
		[Fact]
		public void ShouldReturnBadRequestObjectWithProvidedErrorMessageWhenUnsuccessfulResponseIsCalled()
		{
			//Arrange

			string errorMessage = "I am an error message";

			//Act

			ObjectResult result = _responseService.GenerateUnsuccessfulResponse(errorMessage);

			//Assert

			result.Value.Should().Be(errorMessage);

		}


		[Fact]
		public void ShouldReturnOkObjectWithPassCodeWhenCallingCorrectPasswordMethod()
		{
			//Arrange

			Guid guid = Guid.NewGuid();

			//Act

			ObjectResult result = _responseService.CreateCorrectPasswordResponse(guid);

			//Assert

			string message = result.Value.ToString();
			message.Should().Contain(guid.ToString());

		}


		[Fact]
		public void ShouldReturnBadRequestObjectWithUsefulErrorMessageWhenIncorrectPasswordMethodCaled()
		{
			//Arrange

			//Act

			ObjectResult result = _responseService.CreateIncorrectPasswordResponse();

			//Assert

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public void ShouldReturnObjectWithMessageContainingCorrectPasswordWhenGenerateResponseWithPasswordMethodIsCalled()
        {
            //Arrange

            
            string tableName = "GenerateResponseWithPasswordTable";
            string partitionKey = "APIPassword";
            string password = "startingPassword";

			TestHelper helper = new TestHelper();
            TableClient tableClient = helper.GetTableClient(tableName);
            LoggerFactory logFactory = new LoggerFactory();
            ILogger logger = logFactory.CreateLogger("ResponseServiceTestGeneratePasswordLogger");
            Mock<ITableService> tableService = new Mock<ITableService>();
            tableService.Setup(x => x.GetPassword()).Returns(password);
            
            ResponseService responseService = new ResponseService(logger, tableService.Object);

            //Act

			ObjectResult response = responseService.GenerateResponseWithPassword();

			//Assert
			
            bool correctPasswordPresent = response.Value.ToString().Contains(password);

            Assert.True(correctPasswordPresent);
        }




    }
}
