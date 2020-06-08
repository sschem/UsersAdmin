using UsersAdmin.Core.Model.System;
using UsersAdmin.Data.Repositories;

namespace UsersAdmin.Test.Unit.Repository.System
{
    public class SystemRepositoryDeleteTest : RepositoryBaseDeleteTest<SystemEntity, SystemRepository>
    {
        public SystemRepositoryDeleteTest()
            : base(new SystemRepositoryTest())
        {

        }
    }
}