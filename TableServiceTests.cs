using APIChallenge;
using APIChallengeClassLibrary;
using Azure;
using Azure.Data.Tables;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
	public class TableServiceTests
	{
		private TestHelper _testHelper;
		

        public TableServiceTests()
		{
			_testHelper = new TestHelper();            
		}

		[Fact]
		public void PassCodeTableShouldExistAfterTableServiceIsInstantiated()
		{
			//Arrange
			string connectionString = "UseDevelopmentStorage=true";
			string tableName = "testTableToBeDeleted";			
            string passwordPartitionKey = "partitionKey";
            string password = "password";
			string passCodePartitionKey = "passCodePartition";

			TableClient tableClient = new TableClient(connectionString, tableName);

            //Act

            TableService service = new TableService(tableClient, passCodePartitionKey, passwordPartitionKey,password);

			//Assert

			Response result = tableClient.Delete();
			result.Status.Should().Be(204);

		}

        [Fact]
		public void ShouldWritePassCodeToTable()
		{
			//Arrange
            string tableName = "ShouldWritePassCodeToTable";
            string passwordPartitionKey = "PartitionKey";
            string password = "password";
			string passCodePartitionKey = "passCodePartition";

			TestHelper helper = new TestHelper();
            TableClient tableClient = helper.GetTableClient(tableName);
			TableService tableService = new TableService(tableClient, passCodePartitionKey, passwordPartitionKey, password);

			Guid rowKey = Guid.NewGuid();


			//Act

			tableService.AddPassCodeToTable(rowKey);


			//Assert

			Response<APIChallengeTableEntity> result = tableClient.GetEntity<APIChallengeTableEntity>(passCodePartitionKey, rowKey.ToString());
			result.Value.RowKey.Should().Be(rowKey.ToString());


			//ClearDown

			tableClient.Delete();

		}


		[Fact]
		public void ShouldReturnTrueIfGivenValueIsARowKeyInTheTable()
		{
			//Arrange

			string passCode = "passcodewhichmustbepresent";
			string tableName = "ShouldReturnTrueIfGivenValueIsARowKeyInTheTable";
			string passwordPartitionKey = "PartitionKey";
			string password = "password";
			string passCodePartitionKey = "passCodePartition";

			TestHelper helper = new TestHelper();
			TableClient tableClient = helper.GetTableClient(tableName);
			TableService tableService = new TableService(tableClient, passCodePartitionKey, passwordPartitionKey, password);

			APIChallengeTableEntity entity = new APIChallengeTableEntity() {PartitionKey= passwordPartitionKey, RowKey = passCode };
			tableClient.UpsertEntity<APIChallengeTableEntity>(entity);

			//Act

			bool result = tableService.PassCodeIsInTable(passCode);

			//Assert

			result.Should().BeTrue();

			//ClearDown

			tableClient.Delete();

		}


		[Fact]
		public void ShouldDeletePassCodeFromTable()
		{
			//Arrange

			string passCode = "passcodetobedeleted";
			string tableName = "ShouldDeletePassCodeFromTable";
			string passwordPartitionKey = "PartitionKey";
			string password = "password";
			string passCodePartitionKey = "passCodePartition";

			TestHelper helper = new TestHelper();
			TableClient tableClient = helper.GetTableClient(tableName);
			TableService tableService = new TableService(tableClient, passCodePartitionKey, passwordPartitionKey, password);
			APIChallengeTableEntity entity = new APIChallengeTableEntity() { PartitionKey = passCodePartitionKey, RowKey = passCode };
			tableClient.UpsertEntity<APIChallengeTableEntity>(entity);

			//Act

			tableService.DeletePassCodeFromTable(passCode);

			//Assert
			var tableContents = tableClient.Query<APIChallengeTableEntity>().ToList();
			bool passCodeIsInTable = tableContents.Select(x => x.RowKey).Contains(passCode);
			passCodeIsInTable.Should().BeFalse();


			//ClearDown

			tableClient.Delete();


		}


        [Fact]
        public void ShouldCreatePasswordInTableWhenUpsertPasswordMethodIsCalled()
        {
            //Arrange
            string passwordPartitionKey = "TestPasswordPartitionKey";
            string password = "testPassword";

			string tableName = "ShouldCreatePasswordInTableWhenUpsertPasswordMethodIsCalled";
			string passCodePartitionKey = "passCodePartition";

			TestHelper helper = new TestHelper();
			TableClient tableClient = helper.GetTableClient(tableName);
			TableService tableService = new TableService(tableClient, passCodePartitionKey, passwordPartitionKey, password);

			//Act
			tableService.UpsertPasswordInTable(passwordPartitionKey, password);			

            //Assert
            string actualPassword = helper.GetValueFromTable(tableClient, passwordPartitionKey);
            Assert.Equal(password, actualPassword);

			//ClearDown

			tableClient.Delete();
		}


        [Fact]
        public void ShouldGetPasswordFromTableWhenGetPasswordMethodCalled()
        {
            //Arrange

            string passCodePartitionKey = "PassCodePartitionKey";
            string password = "TestPassword";
            string passwordPartitionKey = "PasswordPartitionKey";

            TableClient tableClient = _testHelper.GetTableClient("GetPasswordFromTable");
            TableService tableService = new TableService(tableClient,passCodePartitionKey, passwordPartitionKey,password);

            //Act
            string returnedPassword = tableService.GetPassword();

            //Assert

            Assert.Equal(password, returnedPassword);
        }


        [Fact]
        public void ShouldReturnPasswordWithMostRecentTimeStampIfMoreThanOnePasswordInTable()
        {
            //Arrange
            string passCodePartitionKey = "passcodePartitionKey";
            string passwordPartitionKey = "passwordPartitionKey";
            string password = "startingPassword";
            string secondPassword = "secondPassword";

            TableClient tableClient = _testHelper.GetTableClient("MostRecentPasswordTable");
            TableService tableService = new TableService(tableClient,passCodePartitionKey, passwordPartitionKey,password);
            _testHelper.AddEntityToTable(tableClient, passwordPartitionKey,secondPassword);

            //Act

            string actualPassword = tableService.GetPassword();

            //Assert
            APIChallengeTableEntity mostRecentEntity = _testHelper.GetMostRecentEntity(tableClient,passwordPartitionKey);

            Assert.Equal(mostRecentEntity.RowKey,actualPassword);

			// CleanDown
			tableClient.Delete();
        }






    }	

}
