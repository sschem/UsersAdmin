using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.User;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.User
{
    public class UserControllerGetTest : ControllerBaseTest
    {
        private readonly UserDto _userDto;

        public UserControllerGetTest()
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
        public async void GetAllUsers_ObtainAtLeastOne()
        {
            _userDto.Id = "Test.GetAllUser.OK";
            this.AddDto<UserEntity, UserDto>(_userDto);

            var response = await _client.GetAsync("/api/Users");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer<IEnumerable<UserItemDto>>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.UserId == _userDto.Id);
        }

        [Fact]
        public async void GetByNameFilter_ObtainOneAtLeast()
        {
            _userDto.Id = "Test.GetByNameFilter";
            this.AddDto<UserEntity, UserDto>(_userDto);

            var response = await _client.GetAsync("/api/Users/filterByName?name=" + _userDto.Id.Substring(0,8));
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(CONTENT_TYPE);

            var answer = JsonConvert.DeserializeObject<Answer<IEnumerable<UserItemDto>>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.UserId == _userDto.Id);
        }
    }
}
