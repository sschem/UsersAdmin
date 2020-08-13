using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Security;
using UsersAdmin.Core.Services;
using UsersAdmin.Services;

namespace UsersAdmin.Test.Unit.Service.User
{
    using static Testing;
    public class UserServiceTest
    {
        protected UserDto GetNewValidDto() => GetValidUserDto();

        protected UserLoginDto GetNewValidLoginDto() => GetValidUserLoginDto();

        protected Mock<IUnitOfWork> GetNewMockedUnitOfWork(IUserRepository repository)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Users).Returns(repository);
            return unitOfWorkMock;
        }

        protected Mock<IUserRepository> GetNewEmptyMockedRepository()
        {
            return new Mock<IUserRepository>();
        }

        protected Mock<ITokenProvider> GetNewEmptyMockedTokenProvider()
        {
            return new Mock<ITokenProvider>();
        }

        protected void ChangeIdToNull(ref UserDto dto)
        {
            dto.Id = null;
        }

        protected (UserService Service, Mock<IUnitOfWork> MockUnitOfWork, Mock<IAppCache> MockCache)
            GetNewService(IUserRepository repository)
        {
           return this.GetNewService(repository, UserRole.Admin.ToString());
        }

        protected (UserService Service, Mock<IUnitOfWork> MockUnitOfWork, Mock<IAppCache> MockCache)
            GetNewService(IUserRepository repository, string role)
        {
            var mockUnitOfWork = this.GetNewMockedUnitOfWork(repository);

            var mockCache = new Mock<IAppCache>();
            mockCache.Setup(c => c.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<UserEntity>>(null));

            var mockTokenProvider = new Mock<ITokenProvider>();
            mockTokenProvider.Setup(t => t.BuildToken(It.IsAny<UserEntity>(), It.IsAny<string>()))
                .Returns(new TokenInfo() { Token = "MockedToken", Role = role });

            return (new UserService(mockUnitOfWork.Object, MapperInstance, mockCache.Object, mockTokenProvider.Object),
                mockUnitOfWork, mockCache);
        }
    }
}
