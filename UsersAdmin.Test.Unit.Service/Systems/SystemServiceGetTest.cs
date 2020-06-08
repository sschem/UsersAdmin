using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Services;
using Xunit;

namespace UsersAdmin.Test.Unit.Service.Systems
{
    using static Testing;

    public class SystemServiceGetTest : SystemServiceTest
    {
        [Fact]
        public async void SystemService_GetByIdAsync_GetOne()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(MapperInstance.Map<SystemEntity>(dto)));
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDto = await serviceMock.Service.GetByIdAsync(new object[] { dto.Id });

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            obtainedDto.Should().NotBeNull();
            obtainedDto.Id.Should().Equals(dto.Id);
            obtainedDto.Name.Should().Equals(dto.Name);
            obtainedDto.Description.Should().Equals(dto.Description);
        }

        [Fact]
        public async void SystemService_GetByIdAsync_NotFound_ThrowException()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(null);
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task<SystemDto>> getByIdFunc = async () => await serviceMock.Service.GetByIdAsync(new object[] { dto.Id });

            await getByIdFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.EntityNotFoundMessage);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public async void SystemService_GetByIdAsync_Null_ThrowException()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.Is<object[]>(o => o == null)))
                .Returns(null);
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task<SystemDto>> getByIdFunc = async () => await serviceMock.Service.GetByIdAsync(null);

            await getByIdFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.EntityNotFoundMessage);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public async void SystemService_GetAllAsync_GetOne()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity> { MapperInstance.Map<SystemEntity>(dto) })
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void SystemService_GetAllAsync_GetEmpty()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity>())
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public async void SystemService_GetAllItemsAsync_GetOne()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity> { MapperInstance.Map<SystemEntity>(dto) })
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllItemsAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void SystemService_GetAllItemsAsync_GetEmpty()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity>())
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllItemsAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public void SystemService_GetWithUsers_GetOne()
        {
            SystemDto systemDto = this.GetNewValidDto();
            UserDto userDto = GetValidUserDto();

            SystemEntity entity = MapperInstance.Map<SystemEntity>(systemDto);
            entity.UserSystemLst.Add(new UserSystemEntity
            {
                SystemId = systemDto.Id,
                System = entity,
                UserId = userDto.Id,
                User = MapperInstance.Map<UserEntity>(userDto)
            });

            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectIncludingUsers(It.IsAny<string>()))
                .Returns(entity);
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDto = serviceMock.Service.GetWithUsers(systemDto.Id);

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectIncludingUsers(It.IsAny<string>()), Times.Once);
            obtainedDto.Should().NotBeNull();
            obtainedDto.Users.Should().NotBeNull();
            obtainedDto.Users.Should().HaveCount(1);
            obtainedDto.Id.Should().Equals(systemDto.Id);
            obtainedDto.Users.First().UserId.Should().Equals(userDto.Id);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("x")]
        public void SystemService_GetWithUsers_GetNull(string systemId)
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectIncludingUsers(It.IsAny<string>()))
                .Returns((SystemEntity)null);
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDto = serviceMock.Service.GetWithUsers(systemId);

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectIncludingUsers(It.IsAny<string>()), Times.Once);
            obtainedDto.Should().BeNull();
        }
    }
}
