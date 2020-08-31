using System;
using System.Collections.Generic;
using System.Text;
using Tatisoft.UsersAdmin.Core.Model.Entities;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Repositories;

namespace Tatisoft.UsersAdmin.Core.Model.User
{
    public class UserSystemEntity : EntityBase, IIds
    {
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public string SystemId { get; set; }
        public SystemEntity System { get; set; }

        public UserRole Role { get; set; }

        public object[] GetIds => new object[] { UserId, SystemId };
    }
}
