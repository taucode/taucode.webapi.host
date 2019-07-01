using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.UpdateFoo;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.Features.UpdateFoo
{
    [TestFixture]
    public class UpdateFooControllerTest : MyTestBase
    {
        private FooId _fooId;

        [SetUp]
        public void SetUp()
        {
            var foo = new Foo("andy", "Andrey");
            this.Repository.Save(foo);
            _fooId = foo.Id;
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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
            var updatedFoo = this.Repository.GetById(_fooId);

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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}?info=id", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            var idString = response.Headers.GetValues("X-Instance-Id").Single();
            var id = new FooId(idString);
            var updatedFoo = this.Repository.GetById(id);

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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}?info=route", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            var idString = response.Headers.GetValues("X-Instance-Id").Single();
            var id = new FooId(idString);
            var route = response.Headers.GetValues("X-Route").Single();
            var updatedFoo = this.Repository.GetById(id);

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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}?info=instance", command).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var instance = JsonConvert.DeserializeObject<GetFooByIdQueryResult>(json);

            var route = response.Headers.GetValues("X-Route").Single();
            var idFromHeader = response.Headers.GetValues("X-Instance-Id").Single();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(route, Is.EqualTo($"api/foos/{_fooId}"));
            Assert.That(idFromHeader, Is.EqualTo(_fooId.ToString()));

            var updatedFoo = this.Repository.GetById(_fooId);

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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
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
            var response = this.Client.PutAsJsonAsync($"api/foos/{_fooId}", command).Result;
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
