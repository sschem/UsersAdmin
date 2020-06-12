using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.System
{
    public class SystemControllerPutTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;

        public SystemControllerPutTest()
        {
            _systemDto = new SystemDto()
            {
                Id = "Test.PutSystem.Id",
                Name = "Test.PutSystem.Name",
                Description = "Test.PutSystem.Description"
            };
        }

        [Fact]
        public async void PutSystem_PutOne()
        {
            this.AddDto<SystemEntity, SystemDto>(_systemDto);
            _systemDto.Name = "UdatedName";
            _systemDto.Description = "UdatedDescription";
            var msgContent = this.CreateMessageContent(_systemDto);

            var response = await _client.PutAsync("/api/Systems/" + _systemDto.Id, msgContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var obtainedEntiy = await this.FindAsync<SystemEntity, SystemDto>(_systemDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            
            obtainedEntiy.Should().NotBeNull();
            obtainedEntiy.Name.Should().Be(_systemDto.Name);
            obtainedEntiy.Description.Should().Be(_systemDto.Description);
        }

        [Fact]
        public async void PutSystem_PutNonExistent()
        {
            _systemDto.Id = "PutSystem_PutNonExistent";
            var msgContent = this.CreateMessageContent(_systemDto);

            var response = await _client.PutAsync("/api/Systems/" + _systemDto.Id, msgContent);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
        }
    }
}