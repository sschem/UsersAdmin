using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersAdmin.Core.Models;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Data.Repositories
{
    public class SystemRepository : RepositoryBase<SystemEntity>, ISystemRepository
    {
        public SystemRepository(AuthDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<SystemEntity>> SelectByUser(string userId)
        {
            Func<SystemEntity, bool> predicate = (system) => system.Name == "x";
            //var preRes = 
            throw new NotImplementedException("To analyze different ways to do it");
        }
        
        // public async Task<IEnumerable<SystemEntity>> GetAllWithMusicsAsync()
        // {
        //     return await AuthDbContext.Systems
        //         .Include(a => a.Musics)
        //         .ToListAsync();
        // }

        // public Task<SystemEntity> GetWithMusicsByIdAsync(int id)
        // {
        //     return AuthDbContext.Systems
        //         .Include(a => a.Musics)
        //         .SingleOrDefaultAsync(a => a.Id == id);
        // }

        private AuthDbContext AuthDbContext
        {
            get { return Context as AuthDbContext; }
        }
    }
}