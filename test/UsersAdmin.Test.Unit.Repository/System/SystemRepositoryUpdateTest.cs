using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Data;
using Tatisoft.UsersAdmin.Data.Repositories;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.System
{
    public class SystemRepositoryUpdateTest : RepositoryBaseUpdateTest<SystemEntity, SystemRepository>
    {
        public SystemRepositoryUpdateTest()
            : base(new SystemRepositoryTest())
        {

        }
    }
}