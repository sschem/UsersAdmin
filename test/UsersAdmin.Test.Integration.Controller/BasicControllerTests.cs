using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller
{
    [Collection("Controller collection")]
    public class BasicControllerTests
    {
        private readonly WebAppFactoryFixture _fixture;

        public BasicControllerTests(WebAppFactoryFixture fixture) 
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("/api/Systems")]
        [InlineData("/api/Users")]
        public async void GetAllEndpoints_AreAnswersOfIEnumerables(string url)
        {
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<IEnumerable<Object>>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
        }
    }
}
