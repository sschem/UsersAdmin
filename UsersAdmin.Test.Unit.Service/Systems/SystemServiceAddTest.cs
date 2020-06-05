using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Services;
using Xunit;

namespace UsersAdmin.Test.Unit.Service.Systems
{
    using static Testing;

    public class SystemServiceAddTest
    {
        [Fact]
        public async void SystemService_AddAsync_ValidateOk()
        {
            SystemDto dto = GetValidSystemDto();
            var repositoryMock = new Mock<ISystemRepository>();
            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);
            
            await service.AddAsync(dto);

            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            unitOfWorkMock.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Once);
        }

        [Fact]
        public async void SystemService_AddAsync_Null_ThrowException()
        {
            SystemDto dto = null;
            var repositoryMock = new Mock<ISystemRepository>();
            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            Func<Task> act = async () => await service.AddAsync(dto);

            await act.Should().ThrowAsync<NullReferenceException>();
            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Never);
            unitOfWorkMock.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Never);
        }

        [Fact]
        public async void SystemService_AddAsync_IdNull_ThrowException()
        {
            SystemDto dto = GetValidSystemDto();
            dto.Id = null;
            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.InsertAsync(It.Is<SystemEntity>(e => e.Id == null)))
                .Throws<ArgumentNullException>();

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);

            Func<Task> act = async () => await service.AddAsync(dto);

            await act.Should().ThrowAsync<ArgumentNullException>();
            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            unitOfWorkMock.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Once);
        }

        [Fact]
        public async void SystemService_AddAsync_Existent_ThrowWarningException()
        {
            SystemDto dto = GetValidSystemDto();
            var repositoryMock = new Mock<ISystemRepository>();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(new SystemEntity() { Id = dto.Id}));

            var unitOfWorkMock = GetMockedUnitOfWorkForSystem(repositoryMock.Object);
            SystemService service = new SystemService(unitOfWorkMock.Object, MapperInstance);
            
            Func<Task> act = async () => await service.AddAsync(dto);

            await act.Should().ThrowAsync<WarningException>().WithMessage(service.EntityAlreadyExists);
            unitOfWorkMock.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            unitOfWorkMock.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Never);
        }
    }
}
