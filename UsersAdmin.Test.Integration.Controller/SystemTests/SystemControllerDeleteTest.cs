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
    public class SystemControllerDeleteTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;

        public SystemControllerDeleteTest(WebAppFactoryFixture fixture) :
            base(fixture)
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
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().DeleteAsync("/api/Systems/" + _systemDto.Id);
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);
            await this.GetOkAnswerChecked(response);
            obtainedEntiy.Should().BeNull();
        }

        [Fact]
        public async void DeleteSystem_NotDelete()
        {
            _systemDto.Id = "Test.DeleteSystem.No";
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().DeleteAsync("/api/Systems/NotExistent");
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);
            await this.GetWarnAnswerChecked(response);
            obtainedEntiy.Should().NotBeNull();
        }
    }
}