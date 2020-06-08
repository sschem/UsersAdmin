using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Services;

namespace UsersAdmin.Test.Unit.Service.Systems
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

        protected (SystemService Service, Mock<IUnitOfWork> MockUnitOfWork) GetNewService(ISystemRepository repository)
        {
            var mockUnitOfWork = this.GetNewMockedUnitOfWork(repository);
            return (new SystemService(mockUnitOfWork.Object, MapperInstance), mockUnitOfWork);
        }
            
    }
}
