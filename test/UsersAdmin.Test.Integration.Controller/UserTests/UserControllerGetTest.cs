using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Services;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.UserTests
{
    [Collection("Controller collection")]
    public class UserControllerGetTest : ControllerBaseTest
    {
        private readonly UserDto _userDto;

        public UserControllerGetTest(WebAppFactoryFixture fixture) :
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
        public async void GetAllUsers_ObtainAtLeastOne()
        {
            _userDto.Id = "Test.GetAllUser.OK";
            await _fixture.ClearCache(UserService.GET_ALL_CACHE_KEY);
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users");
            var answer = await this.GetOkAnswerChecked<IEnumerable<UserItemDto>>(response);
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.UserId == _userDto.Id);
        }

        [Fact]
        public async void GetByNameFilter_ObtainOneAtLeast()
        {
            _userDto.Id = "Test.GetByNameFilter";
            await _fixture.ClearCache(UserService.GET_ALL_CACHE_KEY);
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/filterByName?name=" + _userDto.Id.Substring(0,8));
            var answer = await this.GetOkAnswerChecked<IEnumerable<UserItemDto>>(response);
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.UserId == _userDto.Id);
        }
    }
}
