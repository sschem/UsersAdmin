using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Repositories;

namespace Tatisoft.UsersAdmin.Data.Repositories
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(AuthDbContext context) 
            : base(context)
        { }

        public IEnumerable<UserEntity> SelectItemsByNameFilter(string nameFilter)
        {
            Expression<Func<UserEntity, bool>> predicate = u => 
                !string.IsNullOrEmpty(nameFilter) && 
                u.Name.ToUpper().Contains(nameFilter.ToUpper());
            
            var entities = this.SelectByFilter(predicate);
            return entities;
        }

        public UserEntity SelectIncludingSystems(string userId)
        {
            var entity = this.Context.Users.Where(u =>
                    !string.IsNullOrEmpty(userId)
                    && u.Id.ToUpper() == userId.ToUpper()
                )
                .Include(s => s.UserSystemLst)
                .ThenInclude(us => us.System)
                .FirstOrDefault();

            return entity;
        }

        public void RemoveSystem(UserSystemEntity userSystem)
        {
            this.Context.UsersSystems.Remove(userSystem);
        }

        public void AddSystem(UserSystemEntity userSystem)
        {
            this.Context.UsersSystems.Add(userSystem);
        }
    }
}