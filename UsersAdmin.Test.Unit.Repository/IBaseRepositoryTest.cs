using System;
using System.Collections.Generic;
using System.Text;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Data;

namespace UsersAdmin.Test.Unit.Repository
{
    public interface IBaseRepositoryTest<TEntity, TRepository>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        TEntity GetNewValidEntity();

        TRepository GetNewRepository(AuthDbContext context);

        void AssertAllProperties(TEntity expected, TEntity obtained);

        void ChangeNotIdProperties(ref TEntity entity);

        void ChangeIdToNull(ref TEntity entity);

        void ChangeIdToNonExistent(ref TEntity entity);
    }
}
