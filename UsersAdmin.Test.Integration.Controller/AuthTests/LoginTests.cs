using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Services;
using UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.AuthTests
{
    [Collection("Controller collection")]
    public class LoginTests : ControllerBaseTest
    {
        private readonly UserDto _userDto;
        private readonly SystemDto _systemDto;

        public LoginTests(WebAppFactoryFixture fixture) :
            base(fixture)
        {
            _userDto = new UserDto()
            {
                Id = null,
                Name = "Test.Login.Name",
                Description = "Test.Login.Description",
                Email = "validuser@mail.com",
                Pass = "validclearpass"
            };

            _systemDto = new SystemDto()
            {
                Id = null,
                Name = "Test.Login.Name",
                Description = "Test.Login.Description"
            };
        }

        [Fact]
        public async void Login_LoginAsAdmin()
        {
            _userDto.Id = "Test.Login.AsAdmin";
            _userDto.IsAdmin = true;
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var userLogin = new UserLoginDto() { Id = _userDto.Id, Pass = _userDto.Pass };
            var msgContent = _fixture.CreateMessageContent(userLogin);

            var response = await _fixture.CreateClient().PostAsync("/api/Login/", msgContent);
            var answer = await this.GetOkAnswerChecked<UserLoggedDto>(response);

            answer.Content.Id.Should().Be(_userDto.Id);
            answer.Content.Name.Should().Be(_userDto.Name);
            answer.Content.Role.Should().Be(UserRole.Admin.ToString());
            answer.Content.Token.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("User")]
        [InlineData("SystemAdmin")]
        public async void Login_LoginAsOtherRoles(string role)
        {
            var userSystemEntity = this.GenerateUserSystemFormTest(role);
            await _fixture.AddEntity(userSystemEntity);            
            var msgContent = this.GenerateMessageContentForTest();
            
            var response = await _fixture.CreateClient().PostAsync($"/api/Login/{_systemDto.Id}", msgContent);
            var answer = await this.GetOkAnswerChecked<UserLoggedDto>(response);

            answer.Content.Id.Should().Be(_userDto.Id);
            answer.Content.Name.Should().Be(_userDto.Name);
            answer.Content.Role.Should().Be(role);
            answer.Content.Token.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "x")]
        [InlineData("X", null)]
        [InlineData("X", "x")]
        public async void Login_LoginUserError(string userId, string userPass)
        {
            var userLogin = new UserLoginDto() { Id = userId, Pass = userPass };
            var msgContent = _fixture.CreateMessageContent(userLogin);
            var response = await _fixture.CreateClient().PostAsync("/api/Login/", msgContent);
            await this.GetWarnAnswerChecked(response);
        }

        private UserSystemEntity GenerateUserSystemFormTest(string role)
        {
            _userDto.Id = $"Test.Login.{role}";
            _systemDto.Id = _userDto.Id;
            _userDto.IsAdmin = false;
            UserSystemEntity userSystemEntity = new UserSystemEntity
            {
                SystemId = _systemDto.Id,
                System = _fixture.MapperInstance.Map<SystemEntity>(_systemDto),
                UserId = _userDto.Id,
                User = _fixture.MapperInstance.Map<UserEntity>(_userDto),
                Role = (UserRole)Enum.Parse(typeof(UserRole), role)
            };
            return userSystemEntity;
        }

        public StringContent GenerateMessageContentForTest()
        {
            var userLogin = new UserLoginDto() { Id = _userDto.Id, Pass = _userDto.Pass };
            var msgContent = _fixture.CreateMessageContent(userLogin);
            return msgContent;
        }
    }
}
