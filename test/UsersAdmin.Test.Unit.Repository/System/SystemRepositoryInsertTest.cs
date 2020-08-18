using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Data.Repositories;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.System
{
    public class SystemRepositoryInsertTest : RepositoryBaseInsertTest<SystemEntity, SystemRepository>
    {
        public SystemRepositoryInsertTest() 
            : base(new SystemRepositoryTest())
        {

        }
    }
}
