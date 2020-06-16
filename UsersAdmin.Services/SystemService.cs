using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public class SystemService : ServiceBase<SystemDto, SystemEntity, ISystemRepository>, ISystemService
    {
        protected override ISystemRepository Repository => _unitOfWork.Systems;

       public SystemService(IUnitOfWork unitOfWork, IMapper mapper, IAppCache cache)
            : base(unitOfWork, mapper, cache) { }

        protected override void MapPropertiesForUpdate(SystemEntity outdatedEntity, SystemEntity newEntity)
        {
            outdatedEntity.Name = newEntity.Name;
            outdatedEntity.Description = newEntity.Description;
        }

        public SystemDto GetWithUsers(string systemId)
        {
            var entity = this.Repository.SelectIncludingUsers(systemId);
            var system = _mapper.Map<SystemDto>(entity);
            return system;
        }

        public async Task<IEnumerable<SystemItemDto>> GetAllItemsAsync()
        {
            var entities = await this.GetAllEntitiesAsync();
            var systemItems = _mapper.Map< IEnumerable<SystemItemDto>>(entities);
            return systemItems;
        }
    }
}