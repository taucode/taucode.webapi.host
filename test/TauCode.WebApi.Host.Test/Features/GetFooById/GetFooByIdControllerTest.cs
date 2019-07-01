using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using TauCode.WebApi.Dto;
using TauCode.WebApi.Host.Test.FooManagement.AppHost;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;
using TauCode.WebApi.Host.Test.FooManagement.Persistence.Repositories;

namespace TauCode.WebApi.Host.Test.Features.GetFooById
{
    [TestFixture]
    public class GetFooByIdControllerTest
    {
        private TestServer _server;
        private HttpClient _client;
        private MockFooRepository _repository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repository = new MockFooRepository();
            Startup.Repository = _repository;

            _server = TestServer.Create(builder =>
            {
                var startup = Startup.CreateStartup(true);
                startup.Configuration(builder);
            });
            _client = _server.HttpClient;
        }

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            _repository.Clear();
        }
        
        [Test]
        public void GetFooById_InvalidQuery_ReturnsBadContentResult()
        {
            // Arrange
            var badId = new FooId(Guid.Empty);

            // Act
            var response = _client.GetAsync($"api/foos/{badId}").Result;
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
