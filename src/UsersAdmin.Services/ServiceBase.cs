using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Exceptions;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Core.Services;

namespace Tatisoft.UsersAdmin.Services
{
    public abstract class ServiceBase<TDto, TEntity, TRepository> : IService<TDto, TEntity>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected abstract TRepository Repository { get; }
        protected readonly IMapper _mapper;

        private readonly IAppCache _cache;

        public static readonly string GET_ALL_CACHE_KEY = nameof(GetAllAsync) + "_" + typeof(TDto).Name;

        public virtual string EntityNotFoundMessage { get { return "No se encontraron datos!"; } }
        public virtual string EntityAlreadyExists { get { return "Ya existe un objeto con la misma clave!"; } }

        public ServiceBase(IUnitOfWork unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
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

            _ = _cache.RemoveAsync(GET_ALL_CACHE_KEY);

            return dto;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await this.GetAllEntitiesAsync();
            var dtos =  _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;

        }

        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync()
        {
            var entities = await _cache.GetAsync<IEnumerable<TEntity>>(GET_ALL_CACHE_KEY);
            if (entities == null)
            {
                entities = await this.Repository.SelectAllAsync();
                _ = _cache.AddAsync(GET_ALL_CACHE_KEY, entities);
            }
            return entities;
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

            _ = _cache.RemoveAsync(GET_ALL_CACHE_KEY);
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

            _ = _cache.RemoveAsync(GET_ALL_CACHE_KEY);
        }
    }
}
