using FluentAssertions;
using System.Net;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.AuthTests
{
    [Collection("Controller collection")]
    public class AuthorizationAsUserTests : ControllerBaseTest
    {
        public AuthorizationAsUserTests(WebAppFactoryFixture fixture) :
            base(fixture)
        { }

        [Theory]
        [InlineData("/api/users")]
        [InlineData("/api/users/userId")]
        [InlineData("/api/users/userId/SystemId", HttpStatusCode.OK)]
        [InlineData("/api/users/userId/asocciate/systemId")]
        [InlineData("/api/users/userId/unasocciate/systemId")]
        [InlineData("/api/users/filterByName?name=userName")]
        [InlineData("/api/systems")]
        [InlineData("/api/systems/userId")]
        [InlineData("/api/systems/userId/withUsers")]
        public async void Endpoint_Get_Check(string url, HttpStatusCode expectedHttpCode = HttpStatusCode.Forbidden)
        {
            var response = await _fixture.CreateAuthenticatedAsUserClient().GetAsync(url);
            response.StatusCode.Should().Be(expectedHttpCode);
        }

        [Theory]
        [InlineData("/api/users")]
        [InlineData("/api/systems")]
        public async void Endpoint_Post_AllForbidden(string url)
        {
            var msgContent = _fixture.CreateMessageContent(new UserDto());
            var response = await _fixture.CreateAuthenticatedAsUserClient().PostAsync(url, msgContent);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData("/api/users/userId", HttpStatusCode.OK)]
        [InlineData("/api/systems/systemId")]
        public async void Endpoint_Put_Check(string url, HttpStatusCode expectedHttpCode = HttpStatusCode.Forbidden)
        {
            var msgContent = _fixture.CreateMessageContent(new UserDto());
            var response = await _fixture.CreateAuthenticatedAsUserClient().PutAsync(url, msgContent);
            response.StatusCode.Should().Be(expectedHttpCode);
        }

        [Theory]
        [InlineData("/api/users/userId")]
        [InlineData("/api/systems/systemId")]
        public async void Endpoint_Delete_AllForbidden(string url)
        {
            var response = await _fixture.CreateAuthenticatedAsUserClient().DeleteAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
