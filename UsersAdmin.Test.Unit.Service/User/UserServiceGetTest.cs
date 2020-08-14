using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;
using Xunit;

namespace UsersAdmin.Test.Unit.Service.User
{
    using static Testing;

    public class UserServiceGetTest : UserServiceTest
    {
        [Fact]
        public async void GetItemsByNameFilter_GetOne()
        {
            UserDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<UserEntity>)new List<UserEntity> { MapperInstance.Map<UserEntity>(dto) })
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDtos = await serviceMock.Service.GetItemsByNameFilter(dto.Name);

            serviceMock.MockUnitOfWork.VerifyAll();
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<UserEntity>>()), Times.Once);
            obtainedDtos.Should().NotBeNull();
            obtainedDtos.Should().HaveCount(1);
            obtainedDtos.First().UserId.Should().Be(dto.Id);
            obtainedDtos.First().Name.Should().Be(dto.Name);
        }

        [Fact]
        public async void GetItemsByNameFilter_GetEmpty()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDtos = await serviceMock.Service.GetItemsByNameFilter(null);

            serviceMock.MockUnitOfWork.VerifyAll();
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<UserEntity>>()), Times.Once);
            obtainedDtos.Should().NotBeNull();
            obtainedDtos.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllAsync_GetOne()
        {
            UserDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<UserEntity>)new List<UserEntity> { MapperInstance.Map<UserEntity>(dto) })
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<UserEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void GetAllAsync_GetEmpty()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<UserEntity>)new List<UserEntity>())
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<UserEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllItemsAsync_GetOne()
        {
            UserDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<UserEntity>)new List<UserEntity> { MapperInstance.Map<UserEntity>(dto) })
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllItemsAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<UserEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().HaveCount(1);
        }

        [Fact]
        public async void GetAllItemsAsync_GetEmpty()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult(
                    (IEnumerable<UserEntity>)new List<UserEntity>())
                )
                .Verifiable();
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var dtos = await serviceMock.Service.GetAllItemsAsync();

            serviceMock.MockUnitOfWork.VerifyAll();
            serviceMock.MockCache.Verify(mock => mock.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()), Times.Once);
            serviceMock.MockCache.Verify(mock => mock.AddAsync(It.IsAny<string>(), It.IsAny<IEnumerable<UserEntity>>()), Times.Once);
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }        
    }
}
