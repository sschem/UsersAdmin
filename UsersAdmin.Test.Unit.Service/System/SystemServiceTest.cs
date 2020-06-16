using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;
using UsersAdmin.Services;

namespace UsersAdmin.Test.Unit.Service.System
{
    using static Testing;
    public class SystemServiceTest
    {
        protected SystemDto GetNewValidDto() => GetValidSystemDto();

        protected Mock<IUnitOfWork> GetNewMockedUnitOfWork(ISystemRepository repository)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Systems).Returns(repository);
            return unitOfWorkMock;
        }

        protected Mock<ISystemRepository> GetNewEmptyMockedRepository()
        {
            return new Mock<ISystemRepository>();
        }

        protected void ChangeIdToNull(ref SystemDto dto)
        {
            dto.Id = null;
        }

        protected (SystemService Service, Mock<IUnitOfWork> MockUnitOfWork, Mock<IAppCache> MockCache) 
            GetNewService(ISystemRepository repository)
        {
            var mockUnitOfWork = this.GetNewMockedUnitOfWork(repository);

            var mockCache = new Mock<IAppCache>();
            mockCache.Setup(c => c.GetAsync<IEnumerable<SystemEntity>>(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<SystemEntity>>(null));

            return (new SystemService(mockUnitOfWork.Object, MapperInstance, mockCache.Object), mockUnitOfWork, mockCache);
        }
            
    }
}
