using Microsoft.AspNetCore.Mvc.Rendering;

namespace PeliculasWeb.Models.ViewModels
{
    public class PeliculasVM
    {
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }

        public Pelicula Pelicula { get; set; }
    }
}
