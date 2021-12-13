using APIChallengeClassLibrary;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace APIChallengeTests
{
	
	public class TestHelper
	{
		private List<MemoryStream> _listOfStreamsBeingUsed;
		public TestHelper()
		{
			_listOfStreamsBeingUsed = new List<MemoryStream>();
		}
		public Mock<HttpRequest> GenerateMockHttpRequestWithBody(string body)
		{
			Mock<HttpRequest> request = new Mock<HttpRequest>();

			string json = JsonConvert.SerializeObject(body);
			var byteArray = Encoding.ASCII.GetBytes(json);

			MemoryStream bodyStream = new MemoryStream(byteArray);
			_listOfStreamsBeingUsed.Add(bodyStream);
			bodyStream.Flush();
			bodyStream.Position = 0;

			var mockRequest = new Mock<HttpRequest>();
			mockRequest.Setup(x => x.Body).Returns(bodyStream);

			return mockRequest;

		}

		internal TableClient GetTableClient(string tableName)
		{
			string connectionString = "UseDevelopmentStorage=true";
			return new TableClient(connectionString, tableName);
		}

        internal string GetValueFromTable(TableClient tableClient, string partitionKey)
        {
			
           List<APIChallengeTableEntity> listOfAPIChallengeTableEntities = tableClient.Query<APIChallengeTableEntity>(entity => entity.PartitionKey == partitionKey).ToList();

			return listOfAPIChallengeTableEntities[0].RowKey;

        }

        internal int GetNumberOfRowsForGivenPartitionKeyInTable(TableClient tableClient, string partitonKey)
        {
			Pageable<APIChallengeTableEntity> pageableOfEntriesInTable = tableClient.Query<APIChallengeTableEntity>(entity =>entity.PartitionKey == partitonKey);

			return pageableOfEntriesInTable.Count();

			
        }

		public Stream GenerateStreamFromString(string stringToAddToStream)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(stringToAddToStream);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

        internal void AddEntityToTable(TableClient tableClient, string partitionKey, string rowKey)
        {
            APIChallengeTableEntity tableEntity = new APIChallengeTableEntity();
			tableEntity.PartitionKey = partitionKey;
			tableEntity.RowKey = rowKey;
			tableClient.AddEntity<APIChallengeTableEntity>(tableEntity);
        }

        internal APIChallengeTableEntity GetMostRecentEntity(TableClient tableClient, string partitionKey)
        {
			List<APIChallengeTableEntity> allEntities = tableClient.Query<APIChallengeTableEntity>(x => x.PartitionKey == partitionKey).ToList();

			APIChallengeTableEntity mostRecentEntity = allEntities.First();

			foreach (APIChallengeTableEntity entity in allEntities)
            {
				if(entity.Timestamp > mostRecentEntity.Timestamp)
                {
					mostRecentEntity = entity;
                }
            }

			return mostRecentEntity;

        }
    }
}