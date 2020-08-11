using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller
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
            var response = await _fixture.CreateClient().GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<Answer<IEnumerable<Object>>>(responseString);
            
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.Content.Should().NotBeNull();
        }
    }
}
