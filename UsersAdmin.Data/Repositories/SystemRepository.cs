using Microsoft.EntityFrameworkCore;
using System.Linq;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Data.Repositories
{
    public class SystemRepository : RepositoryBase<SystemEntity>, ISystemRepository
    {
        public SystemRepository(AuthDbContext context)
            : base(context)
        { }

        public SystemEntity SelectIncludingUsers(string systemId)
        {
            var entity = this.Context.Systems.Where(s => 
                    !string.IsNullOrEmpty(systemId) 
                    && s.Id.ToUpper() == systemId.ToUpper()
                )
                .Include(s => s.UserSystemLst)
                .ThenInclude(us => us.User)
                .FirstOrDefault();

            return entity;
        }
    }
}