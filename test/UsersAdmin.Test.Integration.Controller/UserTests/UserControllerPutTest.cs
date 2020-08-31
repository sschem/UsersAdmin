using FluentAssertions;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.UserTests
{
    [Collection("Controller collection")]
    public class UserControllerPutTest : ControllerBaseTest
    {
        private readonly UserDto _userDto;

        public UserControllerPutTest(WebAppFactoryFixture fixture) :
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
        public async void PutUser_PutOne()
        {
            _userDto.Id = "PutUser_PutOne";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            _userDto.Name = "UdatedName";
            _userDto.Description = "UdatedDescription";
            var msgContent = _fixture.CreateMessageContent(_userDto);

            var response = await _fixture.CreateAuthenticatedAsAdminClient().PutAsync("/api/Users/" + _userDto.Id, msgContent);
            var obtainedEntiy = await _fixture.FindAsync<UserEntity, UserDto>(_userDto);
            await this.GetOkAnswerChecked(response);
            
            obtainedEntiy.Id.Should().Be(_userDto.Id);
            obtainedEntiy.Name.Should().Be(_userDto.Name);
            obtainedEntiy.Description.Should().Be(_userDto.Description);
            obtainedEntiy.Pass.Should().Be(_userDto.Pass);
            obtainedEntiy.Email.Should().Be(_userDto.Email);
        }

        [Fact]
        public async void PutUser_PutNonExistent()
        {
            _userDto.Id = "PutUser_PutNonExistent";
            var msgContent = _fixture.CreateMessageContent(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PutAsync("/api/Users/" + _userDto.Id, msgContent);
            await this.GetWarnAnswerChecked(response);
        }
    }
}