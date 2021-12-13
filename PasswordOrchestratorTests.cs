using APIChallengeClassLibrary;
using APIChallengeClassLibrary.PasswordOrchestrator;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
    public class PasswordOrchestratorTests
    {

        [Fact]
        public void ShouldUpdateTableWithNewPasswordWhenUpdatePasswordMethodCalled()
        {
            //Arrange

            string newPassword = "newPassword";
            string passwordPartitionKey = "APIPasswordTest";
            string startingPassword = "startingPassword";
            string tableName = "UpdatePasswordTestTable";
            string passCodePartitionKey = "passCodePartition";

            TestHelper helper = new TestHelper();
            TableClient tableClient = helper.GetTableClient(tableName);
            TableService tableService = new TableService(tableClient,passCodePartitionKey,passwordPartitionKey,startingPassword);
            PasswordOrchestrator passwordOrchestrator = new PasswordOrchestrator(passwordPartitionKey,tableService);

            BodyWithSinglePasswordProperty passwordBody = new BodyWithSinglePasswordProperty() { Password = newPassword };
            string bodyAsString = JsonConvert.SerializeObject(passwordBody);
            Stream body = helper.GenerateStreamFromString(bodyAsString);

            //Act
            passwordOrchestrator.UpdatePassword(body);

            //Assert
            string actualPassword = helper.GetValueFromTable(tableClient, passwordPartitionKey);
            Assert.Equal(newPassword, actualPassword);

            //ClearDown

            tableClient.Delete();
        }


        [Fact]
        public void ShouldOnlyHaveOnePasswordEntryInTableAfterUpdatePasswordMethodIsCalled()
        {
            //Arrange
            string newPassword = "newPassword";
            string passwordPartitionKey = "APIPasswordTest";
            string startingPassword = "startingPassword";
            string tableName = "OnePasswordEntryTestTable";
            string passCodePartitionKey = "passCodePartition";

            TestHelper helper = new TestHelper();
            TableClient tableClient = helper.GetTableClient(tableName);
            TableService tableService = new TableService(tableClient,passCodePartitionKey, passwordPartitionKey, startingPassword);
            PasswordOrchestrator passwordOrchestrator = new PasswordOrchestrator(passwordPartitionKey, tableService);

            BodyWithSinglePasswordProperty passwordBody = new BodyWithSinglePasswordProperty() { Password = newPassword };
            string bodyAsString = JsonConvert.SerializeObject(passwordBody);
            Stream body = helper.GenerateStreamFromString(bodyAsString);

            //Act
            passwordOrchestrator.UpdatePassword(body);

            //Assert

            int numberOfPasswordsInTable = helper.GetNumberOfRowsForGivenPartitionKeyInTable(tableClient,passwordPartitionKey);
            Assert.Equal(1, numberOfPasswordsInTable);

            //ClearDown

            tableClient.Delete();
        }






    }
}
