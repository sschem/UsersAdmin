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
    public class SystemControllerPostTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;

        public SystemControllerPostTest(WebAppFactoryFixture fixture) :
            base(fixture)
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
            var msgContent = _fixture.CreateMessageContent(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync("/api/Systems/", msgContent);
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);
            await this.GetOkAnswerChecked(response, HttpStatusCode.Created);
            obtainedEntiy.Should().NotBeNull();
        }

        [Fact]
        public async void PostSystem_PostExistent()
        {
            _systemDto.Id = "PostSystem_PostExistent";
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            var msgContent = _fixture.CreateMessageContent(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync("/api/Systems/", msgContent);
            await this.GetWarnAnswerChecked(response);
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
            var msgContent = _fixture.CreateMessageContent(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync("/api/Systems/", msgContent);
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);
            await this.GetWarnAnswerChecked(response);
            obtainedEntiy.Should().BeNull();
        }
    }
}