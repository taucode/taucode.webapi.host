using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using TauCode.WebApi.Dto;
using TauCode.WebApi.Host.Test.FooManagement.AppHost;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.UpdateFoo;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;
using TauCode.WebApi.Host.Test.FooManagement.Persistence.Repositories;

namespace TauCode.WebApi.Host.Test.Features.UpdateFoo
{
    [TestFixture]
    public class UpdateFooControllerTest
    {
        private TestServer _server;
        private HttpClient _client;
        private MockFooRepository _repository;
        private FooId _fooId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repository = new MockFooRepository();
            Startup.Repository = _repository;

            _server = TestServer.Create<Startup>();
            _client = _server.HttpClient;
        }

        [SetUp]
        public void SetUp()
        {
            var foo = new Foo("andy", "Andrey");
            _repository.Save(foo);
            _fooId = foo.Id;
        }

        [TearDown]
        public void TearDown()
        {
            _repository.Clear();
        }
        
        [Test]
        public void Update_QueryNothing_UpdatesAndReturnsNoContent()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Undra",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
            var updatedFoo = _repository.GetById(_fooId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(updatedFoo.Code, Is.EqualTo("andy"));
            Assert.That(updatedFoo.Name, Is.EqualTo("Undra"));
        }

        [Test]
        public void Update_QueryId_CreatesAndReturnsNoContent()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Undra",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}?info=id", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            var idString = response.Headers.GetValues("X-Instance-Id").Single();
            var id = new FooId(idString);
            var updatedFoo = _repository.GetById(id);

            Assert.That(id, Is.EqualTo(_fooId));
            Assert.That(updatedFoo.Code, Is.EqualTo("andy"));
            Assert.That(updatedFoo.Name, Is.EqualTo("Undra"));
        }

        [Test]
        public void Update_QueryRoute_CreatesAndReturnsNoContent()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Undra",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}?info=route", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            var idString = response.Headers.GetValues("X-Instance-Id").Single();
            var id = new FooId(idString);
            var route = response.Headers.GetValues("X-Route").Single();
            var updatedFoo = _repository.GetById(id);

            Assert.That(id, Is.EqualTo(_fooId));
            Assert.That(route, Is.EqualTo($"api/foos/{id}"));
            Assert.That(updatedFoo.Code, Is.EqualTo("andy"));
            Assert.That(updatedFoo.Name, Is.EqualTo("Undra"));
        }

        [Test]
        public void Update_QueryInstance_CreatesAndReturnsOk()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Undra",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}?info=instance", command).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var instance = JsonConvert.DeserializeObject<GetFooByIdQueryResult>(json);

            var route = response.Headers.GetValues("X-Route").Single();
            var idFromHeader = response.Headers.GetValues("X-Instance-Id").Single();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(route, Is.EqualTo($"api/foos/{_fooId}"));
            Assert.That(idFromHeader, Is.EqualTo(_fooId.ToString()));

            var updatedFoo = _repository.GetById(_fooId);

            Assert.That(updatedFoo.Code, Is.EqualTo("andy"));
            Assert.That(updatedFoo.Name, Is.EqualTo("Undra"));

            Assert.That(instance.Id, Is.EqualTo(updatedFoo.Id));
            Assert.That(instance.Code, Is.EqualTo(updatedFoo.Code));
            Assert.That(instance.Name, Is.EqualTo(updatedFoo.Name));
        }

        [Test]
        public void Update_BadCommand_ReturnsBadRequestWithValidationError()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var validationErrorResponse = JsonConvert.DeserializeObject<ValidationErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(subReason, Is.EqualTo("Validation"));

            Assert.That(validationErrorResponse.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationErrorResponse.Message, Is.EqualTo("The request is invalid."));

            Assert.That(validationErrorResponse.Failures, Has.Count.EqualTo(1));
            var failure = validationErrorResponse.Failures["name"];
            Assert.That(failure.Code, Is.EqualTo("NotEmptyValidator"));
            Assert.That(failure.Message, Is.EqualTo("'Name' must not be empty."));
        }

        [Test]
        public void Update_BadCommandSpecialCase_ReturnsBadRequestWithValidationError()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Marina",
                Dummy = "Gocha",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var validationErrorResponse = JsonConvert.DeserializeObject<ValidationErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(subReason, Is.EqualTo("Validation"));

            Assert.That(validationErrorResponse.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationErrorResponse.Message, Is.EqualTo("The request is invalid."));

            Assert.That(validationErrorResponse.Failures, Has.Count.EqualTo(1));
            var failure = validationErrorResponse.Failures["[unknown]"];
            Assert.That(failure.Code, Is.EqualTo("NoGochaValidator"));
            Assert.That(failure.Message, Is.EqualTo("'Dummy' should not be 'Gocha'."));
        }

        [Test]
        public void Update_BusinessLogicErrorCommand_ReturnsConflictWithBusinessLogicError()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Wat",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(subReason, Is.EqualTo("BusinessLogic"));

            Assert.That(errorResponse.Code, Is.EqualTo("BusinessLogicError"));
            Assert.That(errorResponse.Message, Is.EqualTo("Foo cannot have name 'Wat'!"));
        }

        [Test]
        public void Update_ForbiddenErrorCommand_ReturnsConflictWithBusinessLogicError()
        {
            // Arrange
            var command = new UpdateFooCommand
            {
                Name = "Ira",
            };

            // Act
            var response = _client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            Assert.That(subReason, Is.EqualTo("Forbidden"));

            Assert.That(errorResponse.Code, Is.EqualTo("ForbiddenError"));
            Assert.That(errorResponse.Message, Is.EqualTo("stop!:)"));
        }
    }
}
