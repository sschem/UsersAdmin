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

    public class SystemServiceRemoveTest : SystemServiceTest
    {
        [Fact]
        public async void Remove_ValidateOk()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(new SystemEntity() { Id = dto.Id }));
            var serviceMock = this.GetNewService(repositoryMock.Object);

            await serviceMock.Service.Remove(new object[] { dto.Id });

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.Delete(It.IsAny<SystemEntity>()), Times.Once);
        }
    }
}
