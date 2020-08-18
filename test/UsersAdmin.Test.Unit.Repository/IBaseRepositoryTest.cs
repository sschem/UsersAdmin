using System;
using System.Collections.Generic;
using System.Text;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Data;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository
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
