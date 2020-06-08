using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository.System
{
    public class SystemRepositoryUpdateTest : RepositoryBaseUpdateTest<SystemEntity, SystemRepository>
    {
        public SystemRepositoryUpdateTest()
            : base(new SystemRepositoryTest())
        {

        }
    }
}