using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller.System
{
    
    [Collection("Controller collection")]
    public class SystemControllerGetTest
    {
        private readonly SystemDto _systemDto;
        private readonly UserDto _userDto;
        private readonly WebAppFactoryFixture _fixture;

        public SystemControllerGetTest(WebAppFactoryFixture fixture)
        {
            _fixture = fixture;

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
            _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _fixture.CreateClient().GetAsync("/api/Systems");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer<IEnumerable<SystemItemDto>>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
            answer.Content.Should().NotBeEmpty();
            answer.Content.Should().Contain(s => s.SystemId == _systemDto.Id);
        }

        [Fact]
        public async void GetById_ObtainOne()
        {
            _systemDto.Id = "Test.GetById.One";
            _fixture.AddDto<SystemEntity, SystemDto>(_systemDto);

            var response = await _fixture.CreateClient().GetAsync("/api/Systems/" + _systemDto.Id);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
            answer.Content.Id.Should().Be(_systemDto.Id);
            answer.Content.Name.Should().Be(_systemDto.Name);
            answer.Content.Description.Should().Be(_systemDto.Description);
            answer.Content.Users.Should().BeEmpty();
        }

        [Fact]
        public async void GetById_ObtainNull()
        {
            var response = await _fixture.CreateClient().GetAsync("/api/Systems/NOT_EXISTS");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);
            answer.Code.Should().Be(Answer.WARN_CODE_DEFAULT);
            answer.IsWarning.Should().Be(true);
            answer.IsError.Should().Be(false);
            answer.Content.Should().BeNull();
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
            _fixture.AddEntity(userSystemEntity);

            var response = await _fixture.CreateClient().GetAsync("/api/Systems/" + _systemDto.Id + "/withUsers");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            
            var answer = JsonConvert.DeserializeObject<Answer<SystemDto>>(responseString);
            answer.Code.Should().Be(Answer.OK_CODE);
            answer.IsWarning.Should().Be(false);
            answer.IsError.Should().Be(false);
            answer.Content.Should().NotBeNull();
            answer.Content.Id.Should().Be(_systemDto.Id);
            answer.Content.Name.Should().Be(_systemDto.Name);
            answer.Content.Description.Should().Be(_systemDto.Description);
            answer.Content.Users.Should().NotBeNull();
            answer.Content.Users.Should().NotBeEmpty();
            answer.Content.Users.Should().Contain(u => u.UserId == _userDto.Id);
        }
    }
}
