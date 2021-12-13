using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIChallengeClassLibrary
{
	public interface ITableService
	{
		void AddPassCodeToTable(Guid guid);
		bool PassCodeIsInTable(string passCode);
		void DeletePassCodeFromTable(string passCode);
		string GetPassword();
	}
	    
    public class TableService :ITableService
	{
		private TableClient _tableClient;
		private string _partitionKey;
        private string _passwordPartitionKey;

        public TableService(TableClient tableClient, string passCodePartitionKey, string passwordPartitionKey, string password)
		{
			_tableClient = tableClient;
			_tableClient.CreateIfNotExists();
			_partitionKey = passCodePartitionKey;
			_passwordPartitionKey = passwordPartitionKey;

			UpsertPasswordInTable(passwordPartitionKey, password);
		}

        public string GetPassword()
        {
			string password = _tableClient.Query<APIChallengeTableEntity>(entity => entity.PartitionKey == _passwordPartitionKey).ToList()[0].RowKey;
			return password;
        }

        internal List<APIChallengeTableEntity> GetCollectionOfTableEntitiesForGivenPartitionKey(string partitionKey)
        {
			return _tableClient.Query<APIChallengeTableEntity>(entity => entity.PartitionKey == partitionKey).ToList();
        }

        internal void DeleteEntityFromTable(string partitionKey, string rowKey)
		{
			_tableClient.DeleteEntity(partitionKey, rowKey);
		}

		public void UpsertPasswordInTable(string partitionKey, string password)
        {
            APIChallengeTableEntity apiEntity = new APIChallengeTableEntity();
			apiEntity.PartitionKey = partitionKey;
			apiEntity.RowKey = password;
            _tableClient.UpsertEntity<APIChallengeTableEntity>(apiEntity);
        }

        public void AddPassCodeToTable(Guid guid)
		{
			APIChallengeTableEntity entity = new APIChallengeTableEntity(_partitionKey, guid);
			_tableClient.UpsertEntityAsync<APIChallengeTableEntity>(entity);
		}

		public void DeletePassCodeFromTable(string passCode)
		{
			_tableClient.DeleteEntity(_partitionKey, passCode);
		}

		public bool PassCodeIsInTable(string passCode)
		{
			List<APIChallengeTableEntity> result = _tableClient.Query<APIChallengeTableEntity>().ToList();
			return result.Select(x => x.RowKey).Contains(passCode);
		}
	}
}