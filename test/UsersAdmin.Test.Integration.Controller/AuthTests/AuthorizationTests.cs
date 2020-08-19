using FluentAssertions;
using System.Net;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.AuthTests
{
    [Collection("Controller collection")]
    public class AuthorizationTests : ControllerBaseTest
    { 
        public AuthorizationTests(WebAppFactoryFixture fixture) :
            base(fixture)
        { }

        [Theory]
        [InlineData("/api/users")]
        [InlineData("/api/users/userId")]
        [InlineData("/api/users/userId/SystemId")]
        [InlineData("/api/users/filterByName?name=userName")]
        [InlineData("/api/systems")]
        [InlineData("/api/systems/userId")]
        [InlineData("/api/systems/userId/withUsers")]
        public async void Endpoint_Get_AsAdmin_Ok(string url)
        {
            var response = await _fixture.CreateAuthenticatedAsAdminClient().GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //[Fact]
        //public async void System_GetWithUsers_AsSystemAdmin_Ok()
        //{
        //    var response = await _fixture.CreateAuthenticatedAsSystemAdminClient().GetAsync("/api/Systems/X/withUsers");
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}

        //[Fact]
        //public async void System_GetWithUsers_AsSystemAdmin_Forbidden()
        //{
        //    var response = await _fixture.CreateAuthenticatedAsSystemAdminClient().GetAsync("/api/Systems/X/withUsers");
        //    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        //}

        //[Fact]
        //public async void System_GetWithUsers_AsUser_Ok()
        //{
        //    var response = await _fixture.CreateAuthenticatedAsUserClient().GetAsync("/api/Systems/X/withUsers");
        //    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        //}

        //[Fact]
        //public async void System_GetWithUsers_AsUser_Forbidden()
        //{
        //    var response = await _fixture.CreateAuthenticatedAsUserClient().GetAsync("/api/Systems/X/withUsers");
        //    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        //}
    }
}
