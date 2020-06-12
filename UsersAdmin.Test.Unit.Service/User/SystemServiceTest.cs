using Moq;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
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

        protected (UserService Service, Mock<IUnitOfWork> MockUnitOfWork) GetNewService(IUserRepository repository)
        {
            var mockUnitOfWork = this.GetNewMockedUnitOfWork(repository);
            return (new UserService(mockUnitOfWork.Object, MapperInstance), mockUnitOfWork);
        }
            
    }
}
