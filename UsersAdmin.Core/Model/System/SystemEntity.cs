using System.ComponentModel.DataAnnotations;
using UsersAdmin.Core.Model.Entities;
using UsersAdmin.Core.Model.Mapping;

namespace UsersAdmin.Core.Model.System
{
    public class SystemEntity : EntityBase, IMapFrom<SystemDto>
    {
        public SystemEntity() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
