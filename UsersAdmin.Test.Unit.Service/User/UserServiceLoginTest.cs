using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Security;
using Xunit;


namespace UsersAdmin.Test.Unit.Service.User
{
    using static Testing;

    public class UserServiceLoginTest : UserServiceTest
    {
        [Fact]
        public async void LoginAsAdmin_OK()
        {
            UserDto dto = this.GetNewValidDto();
            dto.IsAdmin = true;

            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<UserEntity>)new List<UserEntity> { MapperInstance.Map<UserEntity>(dto) })
                )
                .Verifiable();

            var tokenProviderMock = this.GetNewEmptyMockedTokenProvider();
            tokenProviderMock.Setup(t => t.BuildToken(It.IsAny<UserEntity>(), It.IsAny<string>()))
                .Returns(new TokenInfo() { Token = "Token", Role = UserRole.Admin.ToString() })
                .Verifiable();

            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDto = await serviceMock.Service.LoginAsAdminAsync(this.GetNewValidLoginDto());

            serviceMock.MockUnitOfWork.VerifyAll();
            obtainedDto.Should().NotBeNull();
            obtainedDto.Id.Should().Be(dto.Id);
            obtainedDto.Name.Should().Be(dto.Name);
            obtainedDto.Role.Should().Be(UserRole.Admin.ToString());
            obtainedDto.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void LoginAsAdmin_NotFound_ThrowException()
        {
            UserDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult((IEnumerable<UserEntity>)new List<UserEntity>()))
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task<UserLoggedDto>> getValidatedFunc = async () => await serviceMock.Service.LoginAsAdminAsync(this.GetNewValidLoginDto());

            await getValidatedFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.UserIncorrect);
            serviceMock.MockUnitOfWork.VerifyAll();
        }

        [Fact]
        public async void LoginAsAdmin_Null_ThrowException()
        {
            UserDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult((IEnumerable<UserEntity>)new List<UserEntity>()))
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task<UserLoggedDto>> getValidatedFunc = async () => await serviceMock.Service.LoginAsAdminAsync(null);

            await getValidatedFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.UserIncorrect);
            serviceMock.MockUnitOfWork.VerifyAll();
        }

        [Theory]
        [InlineData("User")]
        [InlineData("SystemAdmin")]
        public async void LoginInSystem_OK(string role)
        {
            UserDto dto = this.GetNewValidDto();
            dto.IsAdmin = false;

            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult((IEnumerable<UserEntity>)new List<UserEntity> { MapperInstance.Map<UserEntity>(dto) }))
                .Verifiable();

            repositoryMock.Setup(r => r.SelectIncludingSystems(It.IsAny<string>()))
                .Returns(MapperInstance.Map<UserEntity>(dto))
                .Verifiable();

            var serviceMock = this.GetNewService(repositoryMock.Object, role);

            var obtainedDto = await serviceMock.Service.LoginInSystemAsync(this.GetNewValidLoginDto(), "MockSystem");

            serviceMock.MockUnitOfWork.VerifyAll();
            obtainedDto.Should().NotBeNull();
            obtainedDto.Id.Should().Be(dto.Id);
            obtainedDto.Name.Should().Be(dto.Name);
            obtainedDto.Role.Should().Be(role);
            obtainedDto.Token.Should().NotBeNullOrEmpty();
        }
    }
}
