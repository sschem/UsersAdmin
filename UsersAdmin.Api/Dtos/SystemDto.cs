using System.ComponentModel.DataAnnotations;

namespace UsersAdmin.Api.Dtos
{
    public class SystemDto
    {
        [Required(ErrorMessage = "El campo Id es requerido")]
        [StringLength(20, ErrorMessage = "El campo Id debe tener menos de 20 caracteres")]
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(40, ErrorMessage = "El campo Name debe tener menos de 40 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Description es requerido")]
        [StringLength(80, ErrorMessage = "El campo Description debe tener menos de 80 caracteres")]
        public string Description { get; set; }
    }
}
