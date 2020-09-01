using FluentAssertions;
using System.Net;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.AuthTests
{
    [Collection("Controller collection")]
    public class AuthorizationAsAdminTests : ControllerBaseTest
    { 
        public AuthorizationAsAdminTests(WebAppFactoryFixture fixture) :
            base(fixture)
        { }

        [Theory]
        [InlineData("/api/users")]
        [InlineData("/api/users/userId")]
        [InlineData("/api/users/userId/SystemId")]
        [InlineData("/api/users/userId/associate/systemId")]
        [InlineData("/api/users/userId/unassociate/systemId")]
        [InlineData("/api/users/filterByName?name=userName")]
        [InlineData("/api/systems")]
        [InlineData("/api/systems/userId")]
        [InlineData("/api/systems/userId/withUsers")]
        public async void Endpoint_Get_AllOk(string url)
        {
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/users")]
        [InlineData("/api/systems")]
        public async void Endpoint_Post_AllOk(string url)
        {
            var msgContent = _fixture.CreateMessageContent(new UserDto());
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PostAsync(url, msgContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/users/userId")]
        [InlineData("/api/systems/systemId")]
        public async void Endpoint_Put_AllOk(string url)
        {
            var msgContent = _fixture.CreateMessageContent(new UserDto());
            var response = await _fixture.CreateAuthenticatedAsAdminClient().PutAsync(url, msgContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/users/userId")]
        [InlineData("/api/systems/systemId")]
        public async void Endpoint_Delete_AllOk(string url)
        {
            var response = await _fixture.CreateAuthenticatedAsAdminClient().DeleteAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
