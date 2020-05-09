using System.ComponentModel.DataAnnotations;

namespace UsersAdmin.Core.Models
{
    public class SystemEntity : ABaseEntity
    {
        public SystemEntity() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
