using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeliculasWeb.Models
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre de la película es requerido.")]
        [MaxLength(30, ErrorMessage = "El número máximo de carácteres es 30")]
        public string Nombre { get; set; }
        public byte[] RutaImagen { get; set; }

        [Required(ErrorMessage = "La descripción de la película es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La duración de la película es obligatoria")]
        public int Duracion { get; set; }

        public enum TipoClasificacion
        {
            Siete,Trece,Dieciseis,Dieciocho
        }

        public TipoClasificacion Clasificacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set;}

    }
}
