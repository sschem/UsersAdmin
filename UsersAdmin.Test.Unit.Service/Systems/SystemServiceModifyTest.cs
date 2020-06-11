using Moq;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;
using Xunit;

namespace UsersAdmin.Test.Unit.Service.Systems
{
    public class SystemServiceModifyTest : SystemServiceTest
    {
        [Fact]
        public async void Modify_ValidateOk()
        {
            SystemDto dto = this.GetNewValidDto();
            var repositoryMock = this.GetNewEmptyMockedRepository();
            repositoryMock.Setup(r => r.SelectByIdAsync(It.IsAny<object[]>()))
                .Returns(new ValueTask<SystemEntity>(new SystemEntity() { Id = dto.Id }));
            var serviceMock = this.GetNewService(repositoryMock.Object);

            await serviceMock.Service.Modify(dto, new object[0]);

            serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.SelectByIdAsync(It.IsAny<object[]>()), Times.Once);
            //Entity is updated modifying properties; because first obtain entity from repo.
            //serviceMock.MockUnitOfWork.Verify(mock => mock.Systems.Update(It.IsAny<SystemEntity>()), Times.Once);
        }
    }
}
