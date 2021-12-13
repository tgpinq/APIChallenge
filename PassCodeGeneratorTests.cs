using APIChallengeClassLibrary;
using Azure.Data.Tables;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
	public class PassCodeGeneratorTests
	{
		TestHelper _helper;

		public PassCodeGeneratorTests()
        {
			_helper = new TestHelper();
        }

		[Fact]
		public void ShouldCallTableServiceMethodWhereCorrectPasswordIsProvided()
		{
			//Arrange
			
			string password = "correctPassword";			
			Mock<ITableService> tableService = new Mock<ITableService>();
			tableService.Setup(x => x.GetPassword()).Returns(password);
			PassCodeGenerator generator = new PassCodeGenerator(tableService.Object);
			BodyWithSinglePasswordProperty passCodeBody = new BodyWithSinglePasswordProperty() { Password = password};

			string bodyAsString = JsonConvert.SerializeObject(passCodeBody);
			Stream body = _helper.GenerateStreamFromString(bodyAsString);		
			
			//Act

			generator.GeneratePassCodeAndAddToTable(body);

			//Assert

			tableService.Verify(x => x.AddPassCodeToTable(It.IsAny<Guid>()), Times.Once);

		}


		[Fact]
		public void ShouldThrowExceptionWithUsefulMessageWhereProvidedPasswordIsNotCorrect()
		{
			//Arrange

			string password = "correctPassword";
			string incorrectPassword = "incorrectPassword";
			string exceptionMessage = "";
			Mock<ITableService> tableService = new Mock<ITableService>();
			tableService.Setup(x => x.GetPassword()).Returns(password);
			PassCodeGenerator generator = new PassCodeGenerator(tableService.Object);
			BodyWithSinglePasswordProperty passCodeBody = new BodyWithSinglePasswordProperty() { Password = incorrectPassword };

			string bodyAsString = JsonConvert.SerializeObject(passCodeBody);
			Stream body = _helper.GenerateStreamFromString(bodyAsString);

			//Act

			try
			{
				generator.GeneratePassCodeAndAddToTable(body);

			}
			catch (Exception e)
			{

				exceptionMessage = e.Message;
			}

			//Assert

			exceptionMessage.Should().Contain(incorrectPassword);
		}


		[Fact]
		public void ShouldNotCallAddPassCodeToTableIfPasswordIsNotCorrect()
		{
			//Arrange

			string password = "correctPassword";
			string incorrectPassword = "incorrectPassword";
			Mock<ITableService> tableService = new Mock<ITableService>();
			PassCodeGenerator generator = new PassCodeGenerator(tableService.Object);

			BodyWithSinglePasswordProperty passCodeBody = new BodyWithSinglePasswordProperty() { Password = incorrectPassword };
			string bodyAsString = JsonConvert.SerializeObject(passCodeBody);
			Stream body = _helper.GenerateStreamFromString(bodyAsString);
						
			//Act

			try
			{
				generator.GeneratePassCodeAndAddToTable(body);
			}
			catch (Exception){}


			//Assert

			tableService.Verify(x => x.AddPassCodeToTable(It.IsAny<Guid>()), Times.Never);



		}

	

		
	}
}
