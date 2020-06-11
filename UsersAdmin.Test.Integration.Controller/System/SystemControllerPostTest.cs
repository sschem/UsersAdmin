using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.System
{
    public class SystemControllerPostTest : ControllerBaseTest
    {
        private SystemDto _systemDto;

        public SystemControllerPostTest()
        {
            _systemDto = new SystemDto()
            {
                Id = "Test.PostSystem.Id",
                Name = "Test.PostSystem.Name",
                Description = "Test.PostSystem.Description"
            };
        }

        [Fact]
        public async void PostSystem_PostOne()
        {
            var msgContent = this.CreateMessageContent(_systemDto);

            var response = await _client.PostAsync("/api/Systems/", msgContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var obtainedEntiy = await this.FindAsync<SystemEntity, SystemDto>(_systemDto);
            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            obtainedEntiy.Should().NotBeNull();
        }
    }
}