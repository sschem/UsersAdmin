using System;
using System.Collections.Generic;
using System.Text;
using UsersAdmin.Core.Model.Entities;
using UsersAdmin.Core.Model.System;

namespace UsersAdmin.Core.Model.User
{
    public class UserSystemEntity : EntityBase
    {
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public string SystemId { get; set; }
        public SystemEntity System { get; set; }
    }
}
