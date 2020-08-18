using System;
using System.Collections.Generic;
using System.Text;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Data;
using Tatisoft.UsersAdmin.Data.Repositories;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.System
{
    using static Testing;

    public class SystemRepositoryTest : IBaseRepositoryTest<SystemEntity, SystemRepository>
    {
        public void AssertAllProperties(SystemEntity expected, SystemEntity obtained)
        {
            Assert.Equal(expected.Id, obtained.Id);
            Assert.Equal(expected.Name, obtained.Name);
            Assert.Equal(expected.Description, obtained.Description);
        }

        public void ChangeIdToNonExistent(ref SystemEntity entity)
        {
            entity.Id = "_x";
        }

        public void ChangeIdToNull(ref SystemEntity entity)
        {
            entity.Id = null;
        }

        public void ChangeNotIdProperties(ref SystemEntity entity)
        {
            entity.Name = "AnotherName";
            entity.Description = "AnotherDescription";
        }

        public SystemRepository GetNewRepository(AuthDbContext context)
        {
            return new SystemRepository(context);
        }

        public SystemEntity GetNewValidEntity()
        {
            return GetValidSystemEntity();
        }
    }
}
