using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UsersAdmin.Core.Model.Entities;
using UsersAdmin.Core.Model.Mapping;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Core.Model.System
{
    public class SystemEntity : EntityBase, IMapFrom<SystemDto>, IIds
    {
        public SystemEntity() 
        {
            this.UserSystemLst = new List<UserSystemEntity>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserSystemEntity> UserSystemLst { get; set; }

        public object[] GetIds => new object[] { Id };
    }
}
