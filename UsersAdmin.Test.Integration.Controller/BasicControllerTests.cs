using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using UsersAdmin.Api.Answers;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller
{
    public class BasicControllerTests : ControllerBaseTest
    {
        public BasicControllerTests() { }

        [Theory]
        [InlineData("/api/Systems")]
        [InlineData("/api/Users")]
        public async void GetAllEndpoints_AreAnswersOfIEnumerables_ValidateOk(string url)
        {
            var response = await _client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<Answer<IEnumerable<Object>>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.Content.Should().NotBeNull();
        }
    }
}
