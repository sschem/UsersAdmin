using FluentAssertions;
using System.Net;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.UserTests
{
    [Collection("Controller collection")]
    public class UserControllerPostTest : ControllerBaseTest
    {
        private readonly UserDto _userDto;

        public UserControllerPostTest(WebAppFactoryFixture fixture) :
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
        public async void PostUser_PostOne()
        {
            _userDto.Id = "PostUser_PostOne";
            var msgContent = _fixture.CreateMessageContent(_userDto);
            
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync("/api/Users/", msgContent);
            var obtainedEntiy = await _fixture.FindAsync<UserEntity, UserDto>(_userDto);
            
            await this.GetOkAnswerChecked(response, HttpStatusCode.Created);
            obtainedEntiy.Should().NotBeNull();
        }

        [Fact]
        public async void PostUser_PostExistent()
        {
            _userDto.Id = "PostUser_PostExistent";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var msgContent = _fixture.CreateMessageContent(_userDto);
            
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync("/api/Users/", msgContent);
            
            await this.GetWarnAnswerChecked(response);
        }

        [Theory]
        [InlineData(null, "POST_Name")]
        [InlineData("", "POST_Name")]
        [InlineData("POST_ID_01234567890123456789", "POST_Name")]
        [InlineData("POST_ID_1", null)]
        [InlineData("POST_ID_2", "")]
        [InlineData("POST_ID_3", "POST_Name_0123456789012345678901234567890123456789")]
        public async void PostUser_ValidateProperties(string id, string name)
        {
            _userDto.Id = id;
            _userDto.Name = name;
            var msgContent = _fixture.CreateMessageContent(_userDto);
            
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync("/api/Users/", msgContent);
            var obtainedEntiy = await _fixture.FindAsync<UserEntity, UserDto>(_userDto);
            
            await this.GetWarnAnswerChecked(response);
            obtainedEntiy.Should().BeNull();
        }
    }
}