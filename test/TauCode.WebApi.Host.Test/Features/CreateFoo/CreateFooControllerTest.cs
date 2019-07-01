﻿using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.TestHost;
using TauCode.WebApi.Host.Test.App.AppHost;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.CreateFoo;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.App.Domain.Foos;
using TauCode.WebApi.Host.Test.App.Persistence.Repositories;

namespace TauCode.WebApi.Host.Test.Features.CreateFoo
{
    [TestFixture]
    public class CreateFooControllerTest
    {
        private TestServer _server;
        private HttpClient _client;
        private MockFooRepository _repository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repository = new MockFooRepository();
            //Startup.Repository = _repository;

            _server = TestServer.Create<Startup>();
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
        public void Create_QueryNothing_CreatesAndReturnsNoContent()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "usd",
                Name = "U.S. Dollar",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(_repository.GetAll(), Has.Count.EqualTo(1));
        }

        [Test]
        public void Create_QueryValidationError_ReturnsValidationError()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "usd",
                Name = "U.S. Dollar",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos?info=raise-validation-error", command).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var validationError = JsonConvert.DeserializeObject<ValidationErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            
            Assert.That(validationError.Code, Is.EqualTo("the-code"));
            Assert.That(validationError.Message, Is.EqualTo("Raised validation error as requested"));

            var failure1 = validationError.Failures["ergo"];
            Assert.That(failure1.Code, Is.EqualTo("SomethingWrong"));
            Assert.That(failure1.Message, Is.EqualTo("Something is wrong!"));

            var failure2 = validationError.Failures["magari"];
            Assert.That(failure2.Code, Is.EqualTo("AnotherThingWrong"));
            Assert.That(failure2.Message, Is.EqualTo("Another thing is wrong!"));
        }

        [Test]
        public void Create_QueryId_CreatesAndReturnsNoContent()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "usd",
                Name = "U.S. Dollar",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos?info=id", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            var id = response.Headers.GetValues("X-Instance-Id").Single();
            var foo = _repository.GetById(new FooId(id));
            Assert.That(foo.Code, Is.EqualTo("usd"));
            Assert.That(foo.Name, Is.EqualTo("U.S. Dollar"));
        }

        [Test]
        public void Create_QueryRoute_CreatesAndReturnsNoContent()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "usd",
                Name = "U.S. Dollar",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos?info=route", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            var id = response.Headers.GetValues("X-Instance-Id").Single();
            var route = response.Headers.GetValues("X-Route").Single();
            var foo = _repository.GetById(new FooId(id));
            Assert.That(route, Is.EqualTo($"api/foos/{id}"));
            Assert.That(foo.Code, Is.EqualTo("usd"));
            Assert.That(foo.Name, Is.EqualTo("U.S. Dollar"));
        }

        [Test]
        public void Create_QueryInstance_CreatesAndReturnsCreated()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "usd",
                Name = "U.S. Dollar",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos?info=instance", command).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var instance = JsonConvert.DeserializeObject<GetFooByIdQueryResult>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var location = response.Headers.Location.ToString();
            var id = Regex.Match(location, @"/([\da-f-]+)$").Result("$1");
            var foo = _repository.GetById(new FooId(id));
            var route = response.Headers.GetValues("X-Route").Single();
            var idFromHeader = response.Headers.GetValues("X-Instance-Id").Single();

            Assert.That(id, Is.EqualTo(idFromHeader));
            Assert.That(route, Is.EqualTo($"api/foos/{id}"));

            Assert.That(location, Does.EndWith($"api/foos/{id}"));

            Assert.That(foo.Code, Is.EqualTo("usd"));
            Assert.That(foo.Name, Is.EqualTo("U.S. Dollar"));

            Assert.That(instance.Id, Is.EqualTo(foo.Id));
            Assert.That(instance.Code, Is.EqualTo(foo.Code));
            Assert.That(instance.Name, Is.EqualTo(foo.Name));
        }

        [Test]
        public void Create_BadCommand_ReturnsBadRequestWithValidationError()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "usd1",
                Name = "U.S. Dollar",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var validationErrorResponse = JsonConvert.DeserializeObject<ValidationErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(subReason, Is.EqualTo("Validation"));

            Assert.That(validationErrorResponse.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationErrorResponse.Message, Is.EqualTo("The request is invalid."));

            Assert.That(validationErrorResponse.Failures, Has.Count.EqualTo(1));
            var failure = validationErrorResponse.Failures["code"];
            Assert.That(failure.Code, Is.EqualTo("ExactLengthValidator"));
            Assert.That(failure.Message, Is.EqualTo("'Code' must be 3 characters in length. You entered 4 characters."));
        }

        [Test]
        public void Create_BusinessLogicErrorCommand_ReturnsConflictWithBusinessLogicError()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "wat",
                Name = "Waaat!",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(subReason, Is.EqualTo("BusinessLogic"));

            Assert.That(errorResponse.Code, Is.EqualTo("BusinessLogicError"));
            Assert.That(errorResponse.Message, Is.EqualTo("Foo cannot be wat!"));
        }

        [Test]
        public void Create_ForbiddenErrorCommand_ReturnsConflictWithBusinessLogicError()
        {
            // Arrange
            var command = new CreateFooCommand
            {
                Code = "ira",
                Name = "giocca",
            };

            // Act
            var response = _client.PostAsJsonAsync("api/foos", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            Assert.That(subReason, Is.EqualTo("Forbidden"));

            Assert.That(errorResponse.Code, Is.EqualTo("ForbiddenError"));
            Assert.That(errorResponse.Message, Is.EqualTo("nope:)"));
        }

        [Test]
        public void Create_BodyIsNull_ReturnsBadRequest()
        {
            // Arrange
            CreateFooCommand command = null;

            // Act
            var response = _client.PostAsJsonAsync("api/foos", command).Result;
            var subReason = response.Headers.GetValues("X-Sub-Reason").Single();
            var json = response.Content.ReadAsStringAsync().Result;
            var validationErrorResponse = JsonConvert.DeserializeObject<ValidationErrorResponseDto>(json);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(subReason, Is.EqualTo("Validation"));

            Assert.That(validationErrorResponse.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationErrorResponse.Message, Is.EqualTo("Argument 'command' is null"));
        }
    }
}
