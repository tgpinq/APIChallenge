using Azure.Data.Tables;
using Newtonsoft.Json;
using System;
using System.IO;

namespace APIChallengeClassLibrary
{
	public class PassCodeGenerator
	{
		private ITableService _tableService;        

        public PassCodeGenerator(ITableService tableService)
		{
			_tableService = tableService;
			
		}

		public Guid GeneratePassCodeAndAddToTable(Stream body)
		{
			string requestBody;
			BodyWithSinglePasswordProperty passwordBody;

			try
            {
				requestBody = new StreamReader(body).ReadToEnd();

            }
            catch (Exception e)
            {

                throw new NoBodyException($"There was an issue reading your body, it may be because you don't have one, (does that make you a ghost?). Please check your body exists and is valid Json and try again. The actual error thrown was {e.Message}.");
            }
            if (string.IsNullOrWhiteSpace(requestBody))
            {
				throw new NoBodyException($"Your request did not appear to contain a body. Please ensure it contains a body with the format: {JsonConvert.SerializeObject(new BodyWithSinglePasswordProperty() { Password = "Password goes here" })}"); 
            };
            try
            {
				passwordBody = JsonConvert.DeserializeObject<BodyWithSinglePasswordProperty>(requestBody);

            }
			catch (Exception e)
			{

				throw new NoBodyException($"There was an issue reading your body, it may be because you don't have one, (does that make you a ghost?). Please check your body exists and is valid Json and try again. The actual error thrown was {e.Message}.");
			}

			if (string.IsNullOrWhiteSpace(passwordBody.Password))
			{
				throw new NoPassWordException("No value for password was found in your body. Please submit a json body with the key value pair of Password and the password value.");
			}

			Guid passCode = Guid.NewGuid();
			
			if (passwordBody.Password.ToLower() == _tableService.GetPassword().ToLower())
			{
				_tableService.AddPassCodeToTable(passCode);
			}
			else
			{
				throw new InvalidPassWordException($"Provided password {passwordBody.Password} is not the correct password please try again.");
			}

			return passCode;
		}
	}
}