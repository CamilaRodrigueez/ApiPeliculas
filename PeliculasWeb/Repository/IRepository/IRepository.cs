namespace PeliculasWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        //Método para filtrar películas en una categoría
        Task<IEnumerable<T>> GetAllPeliculasInCategory(string url, int categoriaId);
        //Método buscar pelicula por nombre
        Task<IEnumerable<T>> SearchPelicula(string url, string nombre);

        Task<bool> CreateAsync(string url, T itemCrear, string token);
        Task<bool> UpdateAsync(string url, T itemUpdate, string token);
        Task<bool> DeleteAsync(string url, int id, string token);
    }
}
