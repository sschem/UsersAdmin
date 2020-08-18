using System;
using System.Collections.Generic;
using System.Text;
using Tatisoft.UsersAdmin.Core.Model.Entities;
using Tatisoft.UsersAdmin.Core.Model.System;

namespace Tatisoft.UsersAdmin.Core.Model.User
{
    public class UserSystemEntity : EntityBase
    {
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public string SystemId { get; set; }
        public SystemEntity System { get; set; }

        public UserRole Role { get; set; }
    }
}
