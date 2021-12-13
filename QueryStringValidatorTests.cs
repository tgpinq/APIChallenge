using APIChallengeClassLibrary;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
	public class QueryStringValidatorTests
	{
		QueryStringValidator _validator;
		public QueryStringValidatorTests()
		{
			_validator = new QueryStringValidator();
		}

		[Fact]
		public void ShouldThrowExceptionWithCorrectExceptionMessageForEmptyQueryString()
		{
			//Arrange

			string errorMessage = "";
			Dictionary<string, StringValues> request = new Dictionary<string, StringValues>();

			//Act
			try
			{
				_validator.ValidateQueryString(request);
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}

			//Assert

			errorMessage.Should().Contain("You have not submitted a query in the URL");
		}


		[Theory]
		[InlineData("", "southampton","You did not submit a location key in the URL query")]
		[InlineData("location", "Salisbury", "Sorry you did not provide the correct city")]
		public void ShouldThrowExceptionContainingSpecifiedErrorMessage(string submissionKey, string submissionValue, string expectedErrorSnippet)
		{
			//Arrange

			Dictionary<string, StringValues> submittedQuery = new Dictionary<string, StringValues>();
			submittedQuery.Add(submissionKey,submissionValue);
			string errorMessage = "";

			//Act

			try
			{
				_validator.ValidateQueryString(submittedQuery);
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}

			//Assert

			errorMessage.Should().Contain(expectedErrorSnippet);

		}

		[Fact]
		public void ShouldNotThrowExceptionWhereCorrectAnswerProvidedAsLocation()
		{
			//Arrange

			string errorMessage = "";

			Dictionary<string, StringValues> collection = new Dictionary<string, StringValues>();
			StringValues submission = "Southampton";
			collection.Add("location", submission);

			//Act

			try
			{
				_validator.ValidateQueryString(collection);
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}

			//Assert

			errorMessage.Should().Be("");

		}
	}
}
