using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public abstract class ServiceBase<TDto, TEntity, TRepository> : IService<TDto, TEntity>
        where TEntity : class
        where TRepository : IRepository<TEntity>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected abstract TRepository _repository { get; }
        protected readonly IMapper _mapper;
        protected virtual string EntityNotFoundMessage { get { return "No se encontraron datos!"; } }

        public ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected abstract void MapPropertiesForUpdate(TEntity outdatedEntity, TEntity newEntity);

        protected async Task<TEntity> GetEntityByIdAsync(params object[] idValues)
        {
            var entity = await _repository.SelectByIdAsync(idValues);
            if (entity == null)
            {
                throw new WarningException(this.EntityNotFoundMessage);
            }
            return entity;
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();
            return dto;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.SelectAllAsync();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }

        public async Task<TDto> GetByIdAsync(params object[] idValues)
        {
            var entity = await this.GetEntityByIdAsync(idValues);
            if (entity == null)
            {
                throw new WarningException(this.EntityNotFoundMessage);
            }
            var resDto = _mapper.Map<TDto>(entity);
            return resDto;
        }

        public async Task Modify(TDto modifiedDto, params object[] idValues)
        {
            var obtainedEntity = await this.GetEntityByIdAsync(idValues);
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
            var obtainedEntity = await this.GetEntityByIdAsync(idValues);
            _repository.Delete(obtainedEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}
