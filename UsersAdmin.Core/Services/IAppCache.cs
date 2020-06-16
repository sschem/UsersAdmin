using System.Threading.Tasks;

namespace UsersAdmin.Core.Services
{
    public interface IAppCache
    {
        Task AddAsync<T>(string key, T t);

        Task<T> GetAsync<T>(string key) where T : class;

        Task RemoveAsync(string key);
    }
}
