using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El Nombre es requerido.")]
        [MaxLength(30, ErrorMessage ="El número máximo de caracteres es 30")]
        public string Nombre { get; set; }

        //public DateTime FechaCrecion { get; set; }
    }
}
