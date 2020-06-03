using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public abstract class ServiceBase<TDto, TEntity, TRepository> : IService<TDto, TEntity>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected abstract TRepository Repository { get; }
        protected readonly IMapper _mapper;

        protected virtual string EntityNotFoundMessage { get { return "No se encontraron datos!"; } }
        protected virtual string EntityAlreadyExists { get { return "Ya existe un objeto con la misma clave!"; } }

        public ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected abstract void MapPropertiesForUpdate(TEntity outdatedEntity, TEntity newEntity);

        protected async ValueTask<TEntity> GetEntityValidatedByIdAsync(params object[] idValues)
        {
            var entity = await this.Repository.SelectByIdAsync(idValues);
            if (entity == null)
            {
                throw new WarningException(this.EntityNotFoundMessage);
            }
            return entity;
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var obtainedEntity = await this.Repository.SelectByIdAsync(entity.GetIds);
            if (obtainedEntity != null)
            {
                throw new WarningException(this.EntityAlreadyExists);
            }
            
            await this.Repository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();
            return dto;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await this.Repository.SelectAllAsync();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }

        public async ValueTask<TDto> GetByIdAsync(params object[] idValues)
        {
            var entity = await this.GetEntityValidatedByIdAsync(idValues);
            var resDto = _mapper.Map<TDto>(entity);
            return resDto;
        }

        public async Task Modify(TDto modifiedDto, params object[] idValues)
        {
            var obtainedEntity = await this.GetEntityValidatedByIdAsync(idValues);
            var modifiedEntity = _mapper.Map<TEntity>(modifiedDto);
            this.MapPropertiesForUpdate(obtainedEntity, modifiedEntity);
            //MyRepository.Update(entity); //It's not necessary because obtained entity is connected
            await _unitOfWork.CommitAsync();
        }

        public async Task ModifyConnectedEntity(TEntity entity)
        {
            await _unitOfWork.CommitAsync();
        }

        public async Task Remove(params object[] idValues)
        {
            var obtainedEntity = await this.GetEntityValidatedByIdAsync(idValues);
            this.Repository.Delete(obtainedEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}
