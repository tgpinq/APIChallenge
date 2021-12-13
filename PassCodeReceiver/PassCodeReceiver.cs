using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace APIChallengeClassLibrary.PassCodeReceiver
{
	public class PassCodeReceiver
	{
		private ITableService _tableService;

		public PassCodeReceiver(ITableService tableService)
		{
			_tableService = tableService;
		}

		public bool PassCodeIsValid(Stream body)
		{
			string requestBody = new StreamReader(body).ReadToEndAsync().Result;
            if (string.IsNullOrWhiteSpace(requestBody))
            {
				throw new NoBodyException($"Your request does not appear to contain a body. Please ensure it contains a body which should be in the format: {JsonConvert.SerializeObject(new PassCodeReceiverBody() { PassCode = "PassCode goes here."})}.");
            }
			PassCodeReceiverBody receiver = JsonConvert.DeserializeObject<PassCodeReceiverBody>(requestBody);
            if (string.IsNullOrWhiteSpace(receiver.PassCode))
            {
				throw new NoPassCodeException("No value for passcode was found in your body. Please submit a json body with the key value pair of PassCode and the passcode value.");
            }
			bool codeIsInTable = _tableService.PassCodeIsInTable(receiver.PassCode);
            if (codeIsInTable)
            {
				_tableService.DeletePassCodeFromTable(receiver.PassCode);
			}
			return codeIsInTable;
		}
	}
}
