using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace APIChallengeClassLibrary.PasswordOrchestrator
{
    public class PasswordOrchestrator
    {
        private string _partitionKey;
        private TableService _tableService;

        public PasswordOrchestrator(string partitionKey, TableService tableService)
        {
            _partitionKey = partitionKey;
            _tableService = tableService;
        }

        public void UpdatePassword(Stream requestBody)
        {
            StreamReader streamReader = new StreamReader(requestBody); 
            string bodyString = streamReader.ReadToEndAsync().Result;
            BodyWithSinglePasswordProperty passwordProperty = JsonConvert.DeserializeObject<BodyWithSinglePasswordProperty>(bodyString);
            string newPassword = passwordProperty.Password;

            _tableService.UpsertPasswordInTable(_partitionKey, newPassword);

            List<APIChallengeTableEntity> collectionOfTableEntities = _tableService.GetCollectionOfTableEntitiesForGivenPartitionKey(_partitionKey);

            foreach (var entity in collectionOfTableEntities)
            {
                if (entity.RowKey != newPassword)
                {
                    _tableService.DeleteEntityFromTable(_partitionKey, entity.RowKey);
                }
            }                       
        }
    }
}