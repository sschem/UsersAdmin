using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.Mapping;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Data;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.Factory
{
    public class RepositoryTestHelper
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public readonly IMapper MapperInstance;

        public RepositoryTestHelper(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            MapperInstance = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })
            .CreateMapper();
        }

        public async Task InsertDto<TEntity, TDto>(TDto dto)
            where TEntity : class
        {
            var entity = this.MapperInstance.Map<TEntity>(dto);
            await this.InsertEntity(entity);
        }

        public async Task InsertEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                await dbContext.AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async ValueTask<TEntity> SelectAsync<TEntity, TDto>(TDto dto)
            where TEntity : class, IIds
        {
            TEntity entity = this.MapperInstance.Map<TEntity>(dto);
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                return await dbContext.FindAsync<TEntity>(entity.GetIds);
            }
        }
    }
}
