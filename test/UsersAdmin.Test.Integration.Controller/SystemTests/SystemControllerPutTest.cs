using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.SystemTests
{
    [Collection("Controller collection")]
    public class SystemControllerPutTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;

        public SystemControllerPutTest(WebAppFactoryFixture fixture) :
            base(fixture)
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
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            _systemDto.Name = "UdatedName";
            _systemDto.Description = "UdatedDescription";
            var msgContent = _fixture.CreateMessageContent(_systemDto);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().PutAsync("/api/Systems/" + _systemDto.Id, msgContent);
            var obtainedEntiy = await _fixture.FindAsync<SystemEntity, SystemDto>(_systemDto);
            await this.GetOkAnswerChecked(response);
            
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
            await this.GetWarnAnswerChecked(response);
        }
    }
}