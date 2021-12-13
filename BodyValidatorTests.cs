using APIChallengeClassLibrary;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace APIChallengeTests
{
	public class BodyValidatorTests
	{
		private BodyValidator _bodyValidatory;

		public BodyValidatorTests()
		{
			_bodyValidatory = new BodyValidator();
		}

		[Fact]
		public void BodyShouldThrowCorrectExceptionIfCapitalCityIsNullOrEmpty()
		{
			//Arrange

			string errorMessage = "";
			string expectedErrorMessage = "Body does not contain a required string value for property 'CapitalCity'";
			APIBody body = new APIBody()
			{
				CapitalCity = ""

			};

			//Act

			try
			{
				_bodyValidatory.ValidateBody(body);
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}

			//Assert

			errorMessage.Should().Contain(expectedErrorMessage);
		}

		[Fact]
		public void BodyShouldThrowCorrectExceptionIfDoesNotContainCorrectCapitalCity()
		{
			//Arrange

			string errorMessage = "";
			string expectedErrorMessage = "Value provided as the capital city of Azerbaijan is not correct";
			APIBody body = new APIBody()
			{
				CapitalCity = "southampton"
			
			};

			//Act

			try
			{
				_bodyValidatory.ValidateBody(body);
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}


			//Assert

			errorMessage.Should().Contain(expectedErrorMessage);

		}

		public static IEnumerable<object[]> DateForTheory => 
        new List<object[]>
        {
            new object[] { new List<string>() {"I need friends" }, "Your collection of random strings for property 'ListOfRandomStrings' does not have exactly 4 strings," },
			new object[] { new List<string>() {"I have too many friends", "I have too many friends", "I have too many friends", "I have too many friends", "I have too many friends" }, "Your collection of random strings for property 'ListOfRandomStrings' does not have exactly 4 strings," },
			new object[] { new List<string>() {"I have too many friends", "I have too many friends", "I have too many friends", "I have too many friends" }, "One of your strings in the collection ListOfRandomStrings contains a lower case vowel" }

		};

		[Theory, MemberData(nameof(DateForTheory))]
		[InlineData(null, "You have not provided a collection of strings for the property 'ListOfRandomStrings'")]
		public void ShouldThrowExceptionsWhereListHasIncorrectNumbersOrContents(List<string> listOfItems, string expectedErrorMessage)
		{
			//Arrange

			string errorMessage = "";
			
			APIBody body = new APIBody()
			{
				CapitalCity = "Baku",
				ListOfRandomStrings = listOfItems
			};

			//Act

			try
			{
				_bodyValidatory.ValidateBody(body);
			}
			catch (Exception e)
			{

				errorMessage = e.Message;
			}

			//Assert

			errorMessage.Should().Contain(expectedErrorMessage);

		}

		[Theory]
		[InlineData(0, "Your body must contain an 'Age' Property")]
		[InlineData(5, "Tom was older than that")]
		[InlineData(46, "Tom was younger that that")]
		public void ShouldThrowExceptionWhereIncorrectAgeIsProvided(int age, string expectedErrorMessage)
		{
			//Arrange

			string errorMessage = "";

			APIBody body = new APIBody()
			{
				CapitalCity = "Baku",
				ListOfRandomStrings = new List<string>() { "x", "y", "z", "q"},
				Age = age
			};

			//Act
			try
			{
				_bodyValidatory.ValidateBody(body);
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}

			//Assert

			errorMessage.Should().Contain(expectedErrorMessage);

		}
	}

}
