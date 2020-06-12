using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Unit.Service.System
{
    public class SystemServiceAddTest : SystemServiceTest
    {
        [Fact]
        public async void AddAsync_ValidateOk()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            await serviceMock.Service.AddAsync(dto);

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Once);
        }

        [Fact]
        public async void AddAsync_Null_ThrowException()
        {
            SystemDto dto = null;
            var repositoryMock = this.GetNewEmptyMockedRepository();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task> act = async () => await serviceMock.Service.AddAsync(dto);

            await act.Should().ThrowAsync<NullReferenceException>();
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Never);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Never);
        }

        [Fact]
        public async void AddAsync_IdNull_ThrowException()
        {
            SystemDto dto = this.GetNewValidDto();
            this.ChangeIdToNull(ref dto);
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.InsertAsync(It.Is<SystemEntity>(e => e.Id == null)))
                .Throws<ArgumentNullException>();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task> act = async () => await serviceMock.Service.AddAsync(dto);

            await act.Should().ThrowAsync<ArgumentNullException>();
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Once);
        }

        [Fact]
        public async void AddAsync_Existent_ThrowWarningException()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(new SystemEntity() { Id = dto.Id }));
            var serviceMock = this.GetNewService(repositoryMock.Object);

            Func<Task> act = async () => await serviceMock.Service.AddAsync(dto);

            await act.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.EntityAlreadyExists);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.InsertAsync(It.IsAny<SystemEntity>()), Times.Never);
        }
    }
}
