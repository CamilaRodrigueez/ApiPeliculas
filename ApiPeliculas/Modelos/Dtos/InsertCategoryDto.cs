using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class InsertCategoryDto
    {
        [Required(ErrorMessage ="El Nombre es requerido.")]
        [MaxLength(30, ErrorMessage ="El número máximo de caracteres es 30")]
        public string Nombre { get; set; }

    }
}
