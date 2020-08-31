using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.UserTests
{
    [Collection("Controller collection")]
    public class UserControllerDeleteTest : ControllerBaseTest
    {
        private readonly UserDto _userDto;

        public UserControllerDeleteTest(WebAppFactoryFixture fixture) :
            base(fixture)
        {
            _userDto = new UserDto()
            {
                Id = null,
                Name = "Test.GetUser.Name",
                Description = "Test.GetUser.Description",
                Email = "validuser@mail.com",
                Pass = "validclearpass"
            };
        }

        [Fact]
        public async void DeleteUser_DeleteOne()
        {
            _userDto.Id = "Test.DeleteUser.Ok";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().DeleteAsync("/api/Users/" + _userDto.Id);
            var obtainedEntiy = await _fixture.FindAsync<UserEntity, UserDto>(_userDto);
            await this.GetOkAnswerChecked(response);
            obtainedEntiy.Should().BeNull();
        }

        [Fact]
        public async void DeleteUser_NotDelete()
        {
            _userDto.Id = "Test.DeleteUser.No";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().DeleteAsync("/api/Users/NotExistent");
            var obtainedEntiy = await _fixture.FindAsync<UserEntity, UserDto>(_userDto);
            await this.GetWarnAnswerChecked(response);
            obtainedEntiy.Should().NotBeNull();
        }
    }
}