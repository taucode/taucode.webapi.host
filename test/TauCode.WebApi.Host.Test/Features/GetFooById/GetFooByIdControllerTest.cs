using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.Features.GetFooById
{
    [Ignore("todo")]
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
            var validationError = JsonConvert.DeserializeObject<ValidationErrorDto>(json);
            
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(validationError.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationError.Message, Is.EqualTo("The query is invalid."));

            Assert.That(validationError.Failures, Has.Count.EqualTo(1));

            var failure = validationError.Failures["id"];
            Assert.That(failure.Code, Is.EqualTo("NotEqualValidator"));
            Assert.That(failure.Message, Is.EqualTo($"'Id' must not be equal to '{Guid.Empty}'."));
        }
    }
}
