using FluentAssertions;
using System.Net;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.AuthTests
{
    [Collection("Controller collection")]
    public class AuthorizationAsSystemAdminTests : ControllerBaseTest
    {
        public AuthorizationAsSystemAdminTests(WebAppFactoryFixture fixture) :
            base(fixture)
        { }

        [Theory]
        [InlineData("/api/users", HttpStatusCode.Forbidden)]
        [InlineData("/api/users/userId", HttpStatusCode.Forbidden)]
        [InlineData("/api/users/userId/SystemId", HttpStatusCode.OK)]
        [InlineData("/api/users/userId/associate/systemId", HttpStatusCode.OK)]
        [InlineData("/api/users/userId/unassociate/systemId", HttpStatusCode.OK)]
        [InlineData("/api/users/filterByName?name=userName", HttpStatusCode.Forbidden)]
        [InlineData("/api/systems", HttpStatusCode.Forbidden)]
        [InlineData("/api/systems/userId", HttpStatusCode.OK)]
        [InlineData("/api/systems/SystemId/withUsers", HttpStatusCode.OK)]
        public async void Endpoint_Get_Check(string url, HttpStatusCode expectedHttpCode)
        {
            var response = await _fixture.CreateAuthenticatedAsSystemAdminClient().GetAsync(url);
            response.StatusCode.Should().Be(expectedHttpCode);
        }

        [Theory]
        [InlineData("/api/users", HttpStatusCode.OK)]
        [InlineData("/api/systems", HttpStatusCode.Forbidden)]
        public async void Endpoint_Post_Check(string url, HttpStatusCode expectedHttpCode)
        {
            var msgContent = _fixture.CreateMessageContent(new UserDto());
            var response = await _fixture.CreateAuthenticatedAsSystemAdminClient().PostAsync(url, msgContent);
            response.StatusCode.Should().Be(expectedHttpCode);
        }

        [Theory]
        [InlineData("/api/users/userId")]
        [InlineData("/api/systems/systemId")]
        public async void Endpoint_Put_AllOk(string url)
        {
            var msgContent = _fixture.CreateMessageContent(new UserDto());
            var response = await _fixture.CreateAuthenticatedAsSystemAdminClient().PutAsync(url, msgContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/users/userId", HttpStatusCode.Forbidden)]
        [InlineData("/api/systems/systemId", HttpStatusCode.Forbidden)]
        public async void Endpoint_Delete_AllForbidden(string url, HttpStatusCode expectedHttpCode)
        {
            var response = await _fixture.CreateAuthenticatedAsSystemAdminClient().DeleteAsync(url);
            response.StatusCode.Should().Be(expectedHttpCode);
        }
    }
}
