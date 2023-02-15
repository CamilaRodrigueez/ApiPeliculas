using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        ICollection<Pelicula> GetPeliculas();

        Pelicula GetPelicula(int peliculaId);
        bool ExistsPelicula(string nombre);
        bool ExistsPelicula(int id);
        bool CreatePelicula(Pelicula pelicula);
        bool UpdatePelicula(Pelicula pelicula);
        bool DeletePelicula(Pelicula pelicula);
        bool Guardar();


        //Métodos para buscar películas en categoria y buscar película por nombre

        ICollection<Pelicula> GetAllPeliculasInCategory(int categoriaId);
        ICollection<Pelicula> SearchPelicula(string nombre);
    }
}
