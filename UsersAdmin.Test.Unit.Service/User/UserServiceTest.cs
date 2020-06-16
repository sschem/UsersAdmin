using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;
using UsersAdmin.Services;

namespace UsersAdmin.Test.Unit.Service.User
{
    using static Testing;
    public class UserServiceTest
    {
        protected UserDto GetNewValidDto() => GetValidUserDto();

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

        protected void ChangeIdToNull(ref UserDto dto)
        {
            dto.Id = null;
        }

        protected (UserService Service, Mock<IUnitOfWork> MockUnitOfWork, Mock<IAppCache> MockCache) 
            GetNewService(IUserRepository repository)
        {
            var mockUnitOfWork = this.GetNewMockedUnitOfWork(repository);
            
            var mockCache = new Mock<IAppCache>();
            mockCache.Setup(c => c.GetAsync<IEnumerable<UserEntity>>(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<UserEntity>>(null));
            
            return (new UserService(mockUnitOfWork.Object, MapperInstance, mockCache.Object), mockUnitOfWork, mockCache);
        }
            
    }
}
