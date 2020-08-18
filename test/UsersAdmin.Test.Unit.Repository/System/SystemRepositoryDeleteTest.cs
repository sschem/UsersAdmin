using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Data.Repositories;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.System
{
    public class SystemRepositoryDeleteTest : RepositoryBaseDeleteTest<SystemEntity, SystemRepository>
    {
        public SystemRepositoryDeleteTest()
            : base(new SystemRepositoryTest())
        {

        }
    }
}