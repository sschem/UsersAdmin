using UsersAdmin.Core.Model.System;
using UsersAdmin.Data.Repositories;

namespace UsersAdmin.Test.Unit.Repository.System
{
    public class SystemRepositoryInsertTest : RepositoryBaseInsertTest<SystemEntity, SystemRepository>
    {
        public SystemRepositoryInsertTest() 
            : base(new SystemRepositoryTest())
        {

        }
    }
}
