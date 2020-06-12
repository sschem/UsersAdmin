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
        public void GetItemsByNameFilter_GetOne()
        {
            UserDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectItemsByNameFilter(It.IsAny<string>()))
                .Returns(new List<UserEntity>() { MapperInstance.Map<UserEntity>(dto)  });
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDtos = serviceMock.Service.GetItemsByNameFilter(dto.Id);

            serviceMock.MockUnitOfWork.Verify(mock => mock.Users.SelectItemsByNameFilter(It.IsAny<string>()), Times.Once);
            obtainedDtos.Should().NotBeNull();
            obtainedDtos.Should().HaveCount(1);
            obtainedDtos.First().UserId.Should().Be(dto.Id);
            obtainedDtos.First().Name.Should().Be(dto.Name);
        }

        [Fact]
        public void GetItemsByNameFilter_GetEmpty()
        {
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectItemsByNameFilter(It.IsAny<string>()))
                .Returns((IEnumerable<UserEntity>)null);
            var serviceMock = this.GetNewService(repositoryMock.Object);

            var obtainedDtos = serviceMock.Service.GetItemsByNameFilter(null);

            serviceMock.MockUnitOfWork.Verify(mock => mock.Users.SelectItemsByNameFilter(It.IsAny<string>()), Times.Once);
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
            dtos.Should().NotBeNull();
            dtos.Should().BeEmpty();
        }
    }
}
