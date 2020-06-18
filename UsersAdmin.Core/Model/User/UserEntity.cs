using System.Collections.Generic;
using UsersAdmin.Core.Model.Entities;
using UsersAdmin.Core.Model.Mapping;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Core.Model.User
{
    public class UserEntity : EntityBase, IMapFrom<UserDto>, IIds
    {
        public UserEntity()
        {
            this.UserSystemLst = new List<UserSystemEntity>();
        }

        public string Id { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public UserRole Role { get; set; }
        public List<UserSystemEntity> UserSystemLst { get; set; }

        public object[] GetIds => new object[] { Id };
    }
}
