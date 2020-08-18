using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tatisoft.UsersAdmin.Core.Model.Entities
{
    public abstract class EntityBase
    {
        [NotMapped]
        public string CreatedBy { get; set; }

        [NotMapped]
        public DateTime Created { get; set; }

        [NotMapped]
        public string LastModifiedBy { get; set; }

        [NotMapped]
        public DateTime? LastModified { get; set; }
    }
}
