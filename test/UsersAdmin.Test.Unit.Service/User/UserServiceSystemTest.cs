using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Exceptions;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Repositories;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Service.User
{
    using static Testing;

    public class UserServiceSystemTest : UserServiceTest
    {
        private UserDto _user;
        private SystemDto _system;
        private UserEntity _userEntity;
        private SystemEntity _systemEntity;
        private UserEntity _userEntityWithSystem;
        private Mock<IUserRepository> _repositoryMock;

        public UserServiceSystemTest()
        {
            _user = this.GetNewValidDto();
            _system = this.GetNewValidSystemDto();
            _userEntity = MapperInstance.Map<UserEntity>(_user);
            _systemEntity = MapperInstance.Map<SystemEntity>(_system);
            _userEntityWithSystem = MapperInstance.Map<UserEntity>(_user);
            _userEntityWithSystem.UserSystemLst.Add(new UserSystemEntity()
            {
                SystemId = _system.Id,
                UserId = _user.Id,
                System = _systemEntity,
                User = _userEntity
            });
            this.SetupRepository();
        }

        private void SetupRepository()
        {
            _repositoryMock = this.GetNewEmptyMockedRepository();

            _repositoryMock.Setup(r => r.SelectAllAsync())
                .Returns(Task.FromResult((IEnumerable<UserEntity>)new List<UserEntity> { _userEntity }))
                .Verifiable();

            _repositoryMock.Setup(r => r.SelectIncludingSystems(It.IsAny<string>()))
                .Returns(_userEntityWithSystem)
                .Verifiable();
        }

        [Fact]
        public async void GetBySystemAsync_OK()
        {
            var serviceMock = this.GetNewService(_repositoryMock.Object);

            var obtainedDto = await serviceMock.Service.GetBySystemAsync(_user.Id, _system.Id);

            serviceMock.MockUnitOfWork.VerifyAll();
            obtainedDto.Should().NotBeNull();
            obtainedDto.Id.Should().Be(_user.Id);
            obtainedDto.Name.Should().Be(_user.Name);
            obtainedDto.Email.Should().Be(_user.Email);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "zzz")]
        [InlineData("zzz", null)]
        [InlineData("zzz", "zzz")]
        public async void GetBySystemAsync_Null_Or_Inexistent_ThrowException(string userId, string systemId)
        {
            var serviceMock = this.GetNewService(_repositoryMock.Object);

            Func<Task<UserDto>> getValidatedFunc = async () => await serviceMock.Service.GetBySystemAsync(userId, systemId);

            await getValidatedFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.UserSystemIncorrect);
        }

        [Fact]
        public async void UnassociateUserSystemAsync_OK()
        {
            var serviceMock = this.GetNewService(_repositoryMock.Object);

            await serviceMock.Service.UnassociateUserSystemAsync(_user.Id, _system.Id);

            _repositoryMock.Verify(mock => mock.SelectIncludingSystems(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(mock => mock.Update(It.IsAny<UserEntity>()), Times.Once);
            serviceMock.MockUnitOfWork.Verify(mock => mock.CommitAsync(), Times.Once);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "zzz")]
        [InlineData("zzz", null)]
        [InlineData("zzz", "zzz")]
        public async void UnassociateUserSystemAsync_Null_Or_Inexistent_ThrowException(string userId, string systemId)
        {
            var serviceMock = this.GetNewService(_repositoryMock.Object);

            Func<Task> getValidatedFunc = async () => await serviceMock.Service.UnassociateUserSystemAsync(userId, systemId);

            await getValidatedFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.UserSystemIncorrect);
            _repositoryMock.Verify(mock => mock.SelectIncludingSystems(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void AssociateUserSystemAsync_OK()
        {
            var mockSystemRepository = new Mock<ISystemRepository>();            
            mockSystemRepository.Setup(s => s.SelectByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<SystemEntity>(_systemEntity));

            _repositoryMock.Setup(s => s.SelectByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<UserEntity>(_userEntity));

            _repositoryMock.Setup(r => r.SelectIncludingSystems(It.IsAny<string>()))
                .Returns(_userEntity);

            var serviceMock = this.GetNewService(_repositoryMock.Object, mockSystemRepository.Object);

            await serviceMock.Service.AssociateUserSystemAsync(_user.Id, _system.Id);

            _repositoryMock.Verify(mock => mock.SelectIncludingSystems(It.IsAny<string>()), Times.Once);
            mockSystemRepository.Verify(mock => mock.SelectByIdAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(mock => mock.Update(It.IsAny<UserEntity>()), Times.Once);
            serviceMock.MockUnitOfWork.Verify(mock => mock.CommitAsync(), Times.Once);
        }

        [Fact]
        public async void AssociateUserSystemAsync_AlreadyAssociated()
        {
            var mockSystemRepository = new Mock<ISystemRepository>();
            mockSystemRepository.Setup(s => s.SelectByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<SystemEntity>(_systemEntity));

            var serviceMock = this.GetNewService(_repositoryMock.Object, mockSystemRepository.Object);

            await serviceMock.Service.AssociateUserSystemAsync(_user.Id, _system.Id);

            _repositoryMock.Verify(mock => mock.SelectIncludingSystems(It.IsAny<string>()), Times.Once);
            mockSystemRepository.Verify(mock => mock.SelectByIdAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(mock => mock.Update(It.IsAny<UserEntity>()), Times.Never);
            serviceMock.MockUnitOfWork.Verify(mock => mock.CommitAsync(), Times.Never);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "zzz")]
        [InlineData("zzz", null)]
        [InlineData("zzz", "zzz")]
        public async void AssociateUserSystemAsync_Null_Or_Inexistent_ThrowException(string userId, string systemId)
        {
            var mockSystemRepository = new Mock<ISystemRepository>();
            mockSystemRepository.Setup(s => s.SelectByIdAsync(It.IsAny<string>()))
                .Returns(null);

            _repositoryMock.Setup(r => r.SelectIncludingSystems(It.IsAny<string>()))
                .Returns(_userEntity);

            var serviceMock = this.GetNewService(_repositoryMock.Object, mockSystemRepository.Object);

            Func<Task> getValidatedFunc = async () => await serviceMock.Service.AssociateUserSystemAsync(userId, systemId);

            await getValidatedFunc.Should().ThrowAsync<WarningException>().WithMessage(serviceMock.Service.UserSystemIncorrect);

            _repositoryMock.Verify(mock => mock.SelectIncludingSystems(It.IsAny<string>()), Times.Once);
            mockSystemRepository.Verify(mock => mock.SelectByIdAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(mock => mock.Update(It.IsAny<UserEntity>()), Times.Never);
            serviceMock.MockUnitOfWork.Verify(mock => mock.CommitAsync(), Times.Never);
        }
    }
}
