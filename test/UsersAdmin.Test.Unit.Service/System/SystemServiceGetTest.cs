using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Exceptions;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Service.System
{
    using static Testing;

    public class SystemServiceGetTest : SystemServiceTest
    {
        [Fact]
        public async void GetByIdAsync_GetOne()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(MapperInstance.Map<SystemEntity>(dto)));
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDto = await serviceMock.Service.GetByIdAsync(new object[] { dto.Id });

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            obtainedDto.Should().NotBeNull();
            obtainedDto.Id.Should().Be(dto.Id);
            obtainedDto.Name.Should().Be(dto.Name);
            obtainedDto.Description.Should().Be(dto.Description);
        }

        [Fact]
        public async void GetByIdAsync_NotFound_ThrowException()
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
        public async void GetByIdAsync_Null_ThrowException()
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
        public async void GetAllAsync_GetOne()
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
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<SystemEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<SystemEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void GetAllAsync_GetEmpty()
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
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<SystemEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<SystemEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllItemsAsync_GetOne()
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
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<SystemEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<SystemEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void GetAllItemsAsync_GetEmpty()
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
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<SystemEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<SystemEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public void GetWithUsers_GetOne()
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
            obtainedDto.Id.Should().Be(systemDto.Id);
            obtainedDto.Users.First().UserId.Should().Be(userDto.Id);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("x")]
        public void GetWithUsers_GetNull(string systemId)
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
