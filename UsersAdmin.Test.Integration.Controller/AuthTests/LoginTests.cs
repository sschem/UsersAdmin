using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Services;
using UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.AuthTests
{
    [Collection("Controller collection")]
    public class LoginTests
    {
        private readonly UserDto _userDto;
        private readonly WebAppFactoryFixture _fixture;

        public LoginTests(WebAppFactoryFixture fixture)
        {
            _fixture = fixture;

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
        public async void Login_LoginAsAdminOk()
        {
            _userDto.Id = "Test.Login.AsAdminOk";
            _userDto.Role = "Admin";
            await _fixture.ClearCache(UserService.GET_ALL_CACHE_KEY);
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var userLogin = new UserLoginDto() { Id = _userDto.Id, Pass = _userDto.Pass };
            var msgContent = _fixture.CreateMessageContent(userLogin);

            
            var response = await _fixture.CreateClient().PostAsync("/api/Login/", msgContent);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<UserLoggedDto>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
            answer.Content.Id.Should().Be(_userDto.Id);
            answer.Content.Name.Should().Be(_userDto.Name);
            answer.Content.Role.Should().Be(_userDto.Role);
            answer.Content.Token.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "x")]
        [InlineData("X", null)]
        [InlineData("X", "x")]
        public async void Login_LoginUserError(string userId, string userPass)
        {
            await _fixture.ClearCache(UserService.GET_ALL_CACHE_KEY);
            var userLogin = new UserLoginDto() { Id = userId, Pass = userPass };
            var msgContent = _fixture.CreateMessageContent(userLogin);

            var response = await _fixture.CreateClient().PostAsync("/api/Login/", msgContent);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<UserLoggedDto>>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
            answer.Content.Should().BeNull();
        }
    }
}
