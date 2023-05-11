

using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class UsuarioAccount
    {

       
        [Required(ErrorMessage="El usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(10, MinimumLength =4, ErrorMessage ="La contraseña debe tener entre 4 y 10 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
