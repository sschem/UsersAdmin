using System.Collections.Generic;
using Tatisoft.UsersAdmin.Core.Model.Entities;
using Tatisoft.UsersAdmin.Core.Model.Mapping;
using Tatisoft.UsersAdmin.Core.Repositories;

namespace Tatisoft.UsersAdmin.Core.Model.User
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
        
        public bool IsAdmin { get; set; }
        
        public List<UserSystemEntity> UserSystemLst { get; set; }

        public object[] GetIds => new object[] { Id };
    }
}
