using APIChallengeClassLibrary;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
	public class PasswordRetrieverOrchestratorTests:IClassFixture<OrchestratorTestsFixture>
	{
		private PasswordRetrieverOrchestrator _orchestrator;
		private TestHelper _testHelper;

		public PasswordRetrieverOrchestratorTests(OrchestratorTestsFixture fixture)
		{
			_orchestrator = fixture.Orchestrator;
			_testHelper = fixture.TestHelper;
		}


		

	}

	public class OrchestratorTestsFixture: IDisposable
	{
		public PasswordRetrieverOrchestrator Orchestrator { get; private set; }		
		public TestHelper TestHelper { get; set; }

		public OrchestratorTestsFixture()
		{
			LoggerFactory logFactory = new LoggerFactory();
			ILogger log = logFactory.CreateLogger("OrchestratorTests");
			QueryStringValidator queryStringValidator = new QueryStringValidator() ;
			BodyValidator bodyValidator = new BodyValidator();
			Orchestrator = new PasswordRetrieverOrchestrator(log, queryStringValidator, bodyValidator);
			TestHelper = new TestHelper();
		}


		public void Dispose()
		{
			
		}
	}
}
