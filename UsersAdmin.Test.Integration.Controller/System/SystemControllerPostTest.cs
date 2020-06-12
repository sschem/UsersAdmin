using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.System
{
    public class SystemControllerPostTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;

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

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);

            obtainedEntiy.Should().NotBeNull();
        }

        [Fact]
        public async void PostSystem_PostExistent()
        {
            _systemDto.Id = "PostSystem_PostExistent";
            this.AddDto<SystemEntity, SystemDto>(_systemDto);
            var msgContent = this.CreateMessageContent(_systemDto);

            var response = await _client.PostAsync("/api/Systems/", msgContent);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
        }

        [Theory]
        [InlineData(null, "POST_Name")]
        [InlineData("", "POST_Name")]
        [InlineData("POST_ID_01234567890123456789", "POST_Name")]
        [InlineData("POST_ID_1", null)]
        [InlineData("POST_ID_2", "")]
        [InlineData("POST_ID_3", "POST_Name_0123456789012345678901234567890123456789")]
        public async void PostSystem_ValidateProperties(string id, string name)
        {
            _systemDto.Id = id;
            _systemDto.Name = name;
            var msgContent = this.CreateMessageContent(_systemDto);

            var response = await _client.PostAsync("/api/Systems/", msgContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var obtainedEntiy = await this.FindAsync<SystemEntity, SystemDto>(_systemDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);

            obtainedEntiy.Should().BeNull();
        }
    }
}