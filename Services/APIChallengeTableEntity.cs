using Azure;
using Azure.Data.Tables;
using System;

namespace APIChallengeClassLibrary
{
	public class APIChallengeTableEntity : ITableEntity
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }
		public APIChallengeTableEntity()
		{

		}
		public APIChallengeTableEntity(string partitionKey, Guid guid)
		{
			PartitionKey = partitionKey;
			RowKey = guid.ToString();
		}

	}
}