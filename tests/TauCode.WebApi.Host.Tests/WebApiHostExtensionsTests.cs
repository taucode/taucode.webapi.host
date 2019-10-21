using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace TauCode.WebApi.Host.Tests
{
    [TestFixture]
    public class WebApiHostExtensionsTests
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new Factory();
            _httpClient = factory.CreateClient();

        }


        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ConflictError_ExceptionWithoutCode_ReturnsValidConflictResponse()
        {
            // Arrange


            // Act
            var result = _httpClient.GetAsync("conflict-exception").Result;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(result.Headers.GetValues("X-Payload-Type").Single(), Is.EqualTo("Error"));

            var error = result.Content.ReadAsAsync<ErrorDto>().Result;
            Assert.That(error.Code, Is.EqualTo(typeof(InvalidOperationException).FullName));
            Assert.That(error.Message, Is.EqualTo("Wrong operation."));
        }
    }
}
