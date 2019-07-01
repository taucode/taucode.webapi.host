using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.Features.GetFooById
{
    [TestFixture]
    public class GetFooByIdControllerTest : MyTestBase
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }
        
        [Test]
        public void GetFooById_InvalidQuery_ReturnsBadContentResult()
        {
            // Arrange
            var badId = new FooId(Guid.Empty);

            // Act
            var response = this.Client.GetAsync($"api/foos/{badId}").Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var validationErrorResponse = JsonConvert.DeserializeObject<ValidationErrorResponseDto>(json);
            
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(validationErrorResponse.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationErrorResponse.Message, Is.EqualTo("The query is invalid."));

            Assert.That(validationErrorResponse.Failures, Has.Count.EqualTo(1));

            var failure = validationErrorResponse.Failures["id"];
            Assert.That(failure.Code, Is.EqualTo("NotEqualValidator"));
            Assert.That(failure.Message, Is.EqualTo($"'Id' must not be equal to '{Guid.Empty}'."));
        }
    }
}
