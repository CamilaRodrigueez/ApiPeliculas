using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre de la categoría es obligatoria")]
        public string Nombre { get; set; }

        public DateTime FechaCrecion { get; set; }

    }
}
