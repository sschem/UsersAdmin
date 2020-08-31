using FluentAssertions;
using System.Linq;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.UserTests
{

    [Collection("Controller collection")]
    public class UserControllerSystemTest : ControllerBaseTest
    {
        private readonly SystemDto _systemDto;
        private readonly UserDto _userDto;

        public UserControllerSystemTest(WebAppFactoryFixture fixture) :
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
        public async void GetUserBySystem_ObtainOne()
        {
            _systemDto.Id = "Test.GetUserBySystem";
            _userDto.Id = "Test.GetUserBySystem";
            UserSystemEntity userSystemEntity = new UserSystemEntity
            {
                SystemId = _systemDto.Id,
                System = _fixture.MapperInstance.Map<SystemEntity>(_systemDto),
                UserId = _userDto.Id,
                User = _fixture.MapperInstance.Map<UserEntity>(_userDto)
            };
            await _fixture.AddEntity(userSystemEntity);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/" + _userDto.Id + "/" + _systemDto.Id);
            var answer = await this.GetOkAnswerChecked<UserDto>(response);

            answer.Content.Id.Should().Be(_userDto.Id);
            answer.Content.Name.Should().Be(_userDto.Name);
            answer.Content.Description.Should().Be(_userDto.Description);
            answer.Content.Pass.Should().Be(_userDto.Pass);
            answer.Content.Email.Should().Be(_userDto.Email);
        }

        [Fact]
        public async void AssociateUserSystem_Ok()
        {
            _systemDto.Id = "Ts.AssocUserSys.Ok.S";
            _userDto.Id = "Ts.AssocUserSys.Ok.U";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            await _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/" + _userDto.Id + "/associate/" + _systemDto.Id);
            var obtainedEntiy = await _fixture.FindByEntityAsync<UserSystemEntity>(
                new UserSystemEntity() { SystemId = _systemDto.Id, UserId = _userDto.Id }
                );

            await this.GetOkAnswerChecked(response);
            obtainedEntiy.Should().NotBeNull();
        }

        [Fact]
        public async void UnassociateUserSystem_Ok()
        {
            _systemDto.Id = "Ts.UnassocUsSys.Ok.S";
            _userDto.Id = "Ts.UnassocUsSys.Ok.U";
            UserSystemEntity userSystemEntity = new UserSystemEntity
            {
                SystemId = _systemDto.Id,
                System = _fixture.MapperInstance.Map<SystemEntity>(_systemDto),
                UserId = _userDto.Id,
                User = _fixture.MapperInstance.Map<UserEntity>(_userDto)
            };
            await _fixture.AddEntity(userSystemEntity);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/" + _userDto.Id + "/unassociate/" + _systemDto.Id);
            var obtainedEntiy = await _fixture.FindByEntityAsync<UserSystemEntity>(
                new UserSystemEntity() { SystemId = _systemDto.Id, UserId = _userDto.Id }
                );

            await this.GetOkAnswerChecked(response);
            obtainedEntiy.Should().BeNull();
        }
    }
}
