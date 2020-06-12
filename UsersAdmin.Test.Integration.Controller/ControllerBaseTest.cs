using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Data;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller
{
    using static InitialSetup;

    public abstract class ControllerBaseTest : IAsyncLifetime
    {
        protected readonly string CONTENT_TYPE = "application/json; charset=utf-8";
        protected readonly string MEDIA_TYPE = "application/json";
        protected readonly Encoding ENCODING = Encoding.UTF8;
        protected readonly HttpClient _client;

        private static readonly AsyncLock Mutex = new AsyncLock();
        private static bool _initialized;

        public ControllerBaseTest()
        {
            _client = Factory.CreateClient();
        }

        public virtual async Task InitializeAsync()
        {
            if (_initialized)
                return;

            using (await Mutex.LockAsync())
            {
                if (_initialized)
                    return;

                await Factory.Reset();

                _initialized = true;
            }
        }
        public virtual Task DisposeAsync() => Task.CompletedTask;

        protected void AddDto<TEntity, TDto>(TDto dto)
            where TEntity : class
        {
            this.AddEntity(MapperInstance.Map<TEntity>(dto));
        }

        protected void AddEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            using (var scope = Factory.ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                dbContext.Add(entity);
                dbContext.SaveChanges();
            }
        }

        protected async Task<TEntity> FindAsync<TEntity, TDto>(TDto dto)
            where TEntity : class, IIds
        {
            TEntity entity = MapperInstance.Map<TEntity>(dto);
            using (var scope = Factory.ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                return await dbContext.FindAsync<TEntity>(entity.GetIds);
            }
        }

        protected StringContent CreateMessageContent(object dto)
        {
            var jsonDto = JsonConvert.SerializeObject(dto);
            var msgContent = new StringContent(jsonDto, this.ENCODING, this.MEDIA_TYPE);
            return msgContent;
        }
    }
}
