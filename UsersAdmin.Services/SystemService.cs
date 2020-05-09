using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core;
using UsersAdmin.Core.Models;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public class SystemService : ServiceBase<SystemEntity, ISystemRepository>, ISystemService
    {
        protected override ISystemRepository MyRepository => _unitOfWork.Systems;

        public SystemService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        protected override void MapPropertiesForUpdate(SystemEntity outdatedEntity, SystemEntity newEntity)
        {
            outdatedEntity.Name = newEntity.Name;
            outdatedEntity.Description = newEntity.Description;
        }

        public async Task<IEnumerable<SystemEntity>> GetByUser(string userId)
        {
            return await MyRepository.SelectByUser(userId);
        }
    }
}