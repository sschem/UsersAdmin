using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core;
using UsersAdmin.Core.Exceptions;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public abstract class ServiceBase<TEntity, TRepository> : IService<TEntity>
        where TEntity : class
        where TRepository : IRepository<TEntity>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected abstract TRepository MyRepository { get; }
        protected virtual string EntityNotFoundMessage { get { return "No se encontraron datos!"; } }

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected abstract void MapPropertiesForUpdate(TEntity outdatedEntity, TEntity newEntity);

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await MyRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await MyRepository.SelectAllAsync();
            return entities;
        }

        public async Task<TEntity> GetByIdAsync(params object[] idValues)
        {
            var entity = await MyRepository.SelectByIdAsync(idValues);
            if (entity == null)
            {
                throw new WarningException(this.EntityNotFoundMessage);
            }
            return entity;
        }

        public async Task Modify(TEntity modifiedEntity, params object[] idValues)
        {
            var obtainedEntity = await this.GetByIdAsync(idValues);
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
            var obtainedEntity = await this.GetByIdAsync(idValues);
            MyRepository.Delete(obtainedEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}
