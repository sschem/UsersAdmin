using Moq;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.System;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Service.System
{
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
