namespace PeliculasWeb.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<Categoria> ListaCategorias { get; set; }
        public IEnumerable<Pelicula> ListaPeliculas { get; set; }
    }
}
