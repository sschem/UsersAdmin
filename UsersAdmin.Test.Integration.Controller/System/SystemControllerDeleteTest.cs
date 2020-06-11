using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.System
{
    public class SystemControllerDeleteTest : ControllerBaseTest
    {
        private SystemDto _systemDto;

        public SystemControllerDeleteTest()
        {
            _systemDto = new SystemDto()
            {
                Id = null,
                Name = "Test.DeleteSystem.Name",
                Description = "Test.DeleteSystem.Description"
            };
        }

        [Fact]
        public async void DeleteSystem_DeleteOne()
        {
            _systemDto.Id = "Test.DeleteSystem.Ok";
            this.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _client.DeleteAsync("/api/Systems/" + _systemDto.Id);
            var responseString = await response.Content.ReadAsStringAsync();
            var obtainedEntiy = await this.FindAsync<SystemEntity, SystemDto>(_systemDto);
            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            obtainedEntiy.Should().BeNull();
        }

        [Fact]
        public async void DeleteSystem_NotDelete()
        {
            _systemDto.Id = "Test.DeleteSystem.No";
            this.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _client.DeleteAsync("/api/Systems/NotExistent");
            var responseString = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);

            await Task.Delay(5000);
            var obtainedEntiy = await this.FindAsync<SystemEntity, SystemDto>(_systemDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
            obtainedEntiy.Should().NotBeNull();
        }
    }
}