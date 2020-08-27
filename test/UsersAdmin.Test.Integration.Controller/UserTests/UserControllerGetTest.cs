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
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users");
            var answer = await this.GetOkAnswerChecked<IEnumerable<UserItemDto>>(response);
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.UserId == _userDto.Id);
        }

        [Fact]
        public async void GetById_ObtainOne()
        {
            _userDto.Id = "Test.GetById.One";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/" + _userDto.Id);
            var answer = await this.GetOkAnswerChecked<UserDto>(response);
            answer.Content.Id.Should().Be(_userDto.Id);
            answer.Content.Name.Should().Be(_userDto.Name);
            answer.Content.Description.Should().Be(_userDto.Description);
            answer.Content.Pass.Should().Be(_userDto.Pass);
            answer.Content.Email.Should().Be(_userDto.Email);
        }

        [Fact]
        public async void GetById_ObtainNull()
        {
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/NOT_EXISTS");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            await this.GetWarnAnswerChecked(response);
        }

        [Fact]
        public async void GetByNameFilter_ObtainOneAtLeast()
        {
            _userDto.Id = "Test.GetByNameFilter";
            await _fixture.AddDto<UserEntity, UserDto>(_userDto);
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync("/api/Users/filterByName?name=" + _userDto.Id.Substring(0,8));
            var answer = await this.GetOkAnswerChecked<IEnumerable<UserItemDto>>(response);
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.UserId == _userDto.Id);
        }
    }
}
