using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.SystemTests
{
    [Collection("Controller collection")]
    public class SystemControllerPutTest
    {
        private readonly SystemDto _systemDto;
        private readonly WebAppFactoryFixture _fixture;

        public SystemControllerPutTest(WebAppFactoryFixture fixture)
        {
            _fixture = fixture;

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
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            _systemDto.Name = "UdatedName";
            _systemDto.Description = "UdatedDescription";
            var msgContent = _fixture.CreateMessageContent(_systemDto);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().PutAsync("/api/Systems/" + _systemDto.Id, msgContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);

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
            var msgContent = _fixture.CreateMessageContent(_systemDto);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().PutAsync("/api/Systems/" + _systemDto.Id, msgContent);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
        }
    }
}