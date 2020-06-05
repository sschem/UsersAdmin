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

    public class SystemServiceGetTest
    {
        [Fact]
        public async void SystemService_GetByIdAsync_GetOne()
        {
            SystemDto dto = GetValidSystemDto();

            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(MapperInstance.Map<SystemEntity>(dto)));

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            var obtainedDto = await service.GetByIdAsync(new object[] { dto.Id });

            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            obtainedDto.Should().NotBeNull();
            obtainedDto.Id.Should().Equals(dto.Id);
            obtainedDto.Name.Should().Equals(dto.Name);
            obtainedDto.Description.Should().Equals(dto.Description);
        }

        [Fact]
        public async void SystemService_GetByIdAsync_NotFound_ThrowException()
        {
            SystemDto dto = GetValidSystemDto();
            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(null);

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            Func<Task<SystemDto>> getByIdFunc = async () => await service.GetByIdAsync(new object[] { dto.Id });

            await getByIdFunc.Should().ThrowAsync<WarningException>().WithMessage(service.EntityNotFoundMessage);
            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public async void SystemService_GetByIdAsync_IdNull_ThrowException()
        {
            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.Is<object[]>(o => o == null)))
                .Returns(null);

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            Func<Task<SystemDto>> getByIdFunc = async () => await service.GetByIdAsync(null);

            await getByIdFunc.Should().ThrowAsync<WarningException>().WithMessage(service.EntityNotFoundMessage);
            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public async void SystemService_GetAllAsync_GetOne()
        {
            SystemDto dto = GetValidSystemDto();

            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity> { MapperInstance.Map<SystemEntity>(dto) })
                )
                .Verifiable();

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            var dtos = await service.GetAllAsync();

            unitOfWorkMock.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void SystemService_GetAllAsync_GetEmpty()
        {
            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity>())
                )
                .Verifiable();

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            var dtos = await service.GetAllAsync();

            unitOfWorkMock.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public async void SystemService_GetAllItemsAsync_GetOne()
        {
            SystemDto dto = GetValidSystemDto();

            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity> { MapperInstance.Map<SystemEntity>(dto) })
                )
                .Verifiable();

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            var dtos = await service.GetAllItemsAsync();

            unitOfWorkMock.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void SystemService_GetAllItemsAsync_GetEmpty()
        {
            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<SystemEntity>)new List<SystemEntity>())
                )
                .Verifiable();

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            var dtos = await service.GetAllItemsAsync();

            unitOfWorkMock.VerifyAll();
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public void SystemService_GetWithUsers_GetOne()
        {
            SystemDto systemDto = GetValidSystemDto();
            UserDto userDto = GetValidUserDto();

            SystemEntity entity = MapperInstance.Map<SystemEntity>(systemDto);
            entity.UserSystemLst.Add(new UserSystemEntity
            {
                SystemId = systemDto.Id,
                System = entity,
                UserId = userDto.Id,
                User = MapperInstance.Map<UserEntity>(userDto)
            });

            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectIncludingUsers(It.IsAny<string>()))
                .Returns(entity);

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            var obtainedDto = service.GetWithUsers(systemDto.Id);

            unitOfWorkMock.Verify(mock => mock.Systems.SelectIncludingUsers(It.IsAny<string>()), Times.Once);
            obtainedDto.Should().NotBeNull();
            obtainedDto.Users.Should().NotBeNull();
            obtainedDto.Users.Should().HaveCount(1);
            obtainedDto.Id.Should().Equals(systemDto.Id);
            obtainedDto.Users.First().UserId.Should().Equals(userDto.Id);

        }
    }
}
