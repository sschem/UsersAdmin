using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Services;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.SystemTests
{

    [Collection("Controller collection")]
    public class SystemControllerGetTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;
        private readonly UserDto _userDto;

        public SystemControllerGetTest(WebAppFactoryFixture fixture) :
            base(fixture)
        {
            _systemDto = new SystemDto()
            {
                Id = null,
                Name = "Test.GetSystem.Name",
                Description = "Test.GetSystem.Description"
            };

            _userDto = new UserDto()
            {
                Id = null,
                Name = "Test.GetSystem.Name",
                Description = "Test.GetSystem.Description",
                Email = "validuser@mail.com",
                Pass = "validclearpass"
            };
        }

        [Fact]
        public async void GetAllSystems_ObtainAtLeastOne()
        {
            _systemDto.Id = "Test.GetAllSystem.OK";
            await _fixture.ClearCache(SystemService.GET_ALL_CACHE_KEY);
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Systems");
            var answer = await this.GetOkAnswerChecked<IEnumerable<SystemItemDto>>(response);
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.SystemId == _systemDto.Id);
        }

        [Fact]
        public async void GetById_ObtainOne()
        {
            _systemDto.Id = "Test.GetById.One";
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Systems/" + _systemDto.Id);
            var answer = await this.GetOkAnswerChecked<SystemDto>(response);
            answer.Content.Id.Should().Be(_systemDto.Id);
            answer.Content.Name.Should().Be(_systemDto.Name);
            answer.Content.Description.Should().Be(_systemDto.Description);
            answer.Content.Users.Should().BeEmpty();
        }

        [Fact]
        public async void GetById_ObtainNull()
        {
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Systems/NOT_EXISTS");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            await this.GetWarnAnswerChecked(response);
        }

        [Fact]
        public async void GetWithUsers_ObtainOneWithOneUserAtLeast()
        {
            _systemDto.Id = "Test.GetWithUsers.One";
            _userDto.Id = "Test.GetWithUsers.One";
            UserSystemEntity userSystemEntity = new UserSystemEntity
            {
                SystemId = _systemDto.Id,
                System = _fixture.MapperInstance.Map<SystemEntity>(_systemDto),
                UserId = _userDto.Id,
                User = _fixture.MapperInstance.Map<UserEntity>(_userDto)
            };
            await _fixture.AddEntity(userSystemEntity);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Systems/" + _systemDto.Id + "/withUsers");
            var answer = await this.GetOkAnswerChecked<SystemDto>(response);

            answer.Content.Id.Should().Be(_systemDto.Id);
            answer.Content.Name.Should().Be(_systemDto.Name);
            answer.Content.Description.Should().Be(_systemDto.Description);
            answer.Content.Users.Should().NotBeNull();
            answer.Content.Users.Should().NotBeEmpty();
            answer.Content.Users.Should().Contain(u => u.UserId == _userDto.Id);
        }
    }
}
