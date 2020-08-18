using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tatisoft.UsersAdmin.Core.Model.Entities;
using Tatisoft.UsersAdmin.Core.Model.Mapping;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Repositories;

namespace Tatisoft.UsersAdmin.Core.Model.System
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
