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
        protected override ISystemRepository _repository => _unitOfWork.Systems;


        public SystemService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        protected override void MapPropertiesForUpdate(SystemEntity outdatedEntity, SystemEntity newEntity)
        {
            outdatedEntity.Name = newEntity.Name;
            outdatedEntity.Description = newEntity.Description;
        }

        public async Task<IEnumerable<SystemDto>> GetByUserAsync(string userId)
        {
            throw new NotImplementedException();
            //return await MyRepository.SelectByUser(userId);
        }

        public async Task<IEnumerable<SystemItemDto>> GetAllItemsAsync()
        {
            var entities = await _repository.SelectAllAsync();
            var systemItems = _mapper.Map< IEnumerable<SystemItemDto>>(entities);
            return systemItems;
        }
    }
}