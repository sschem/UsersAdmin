using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.System
{
    [Collection("Controller collection")]
    public class SystemControllerDeleteTest
    {
        private readonly SystemDto _systemDto;
        private readonly WebAppFactoryFixture _fixture;

        public SystemControllerDeleteTest(WebAppFactoryFixture fixture)
        {
            _fixture = fixture;

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
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _fixture.CreateClient().DeleteAsync("/api/Systems/" + _systemDto.Id);
            var responseString = await response.Content.ReadAsStringAsync();
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            
            obtainedEntiy.Should().BeNull();
        }

        [Fact]
        public async void DeleteSystem_NotDelete()
        {
            _systemDto.Id = "Test.DeleteSystem.No";
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _fixture.CreateClient().DeleteAsync("/api/Systems/NotExistent");
            var responseString = await response.Content.ReadAsStringAsync();

            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
            
            obtainedEntiy.Should().NotBeNull();
        }
    }
}