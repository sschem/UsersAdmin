using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Data.Repositories
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(AuthDbContext context) 
            : base(context)
        { }

        public IEnumerable<UserEntity> SelectItemsByNameFilter(string nameFilter)
        {
            Expression<Func<UserEntity, bool>> predicate = u => u.Name.ToUpper().Contains(nameFilter.ToUpper());
            var entities = this.SelectByFilter(predicate);
            return entities;
        }
    }
}