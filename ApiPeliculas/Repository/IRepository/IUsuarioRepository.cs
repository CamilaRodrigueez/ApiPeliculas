using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();

        Categoria GetCategoria(int idCategoria);
        bool ExistsCategory(string nombre);
        bool ExistsCategory(int id);
        bool CreateCategory(Categoria categoria);
        bool UpdateCategory(Categoria categoria);
        bool DeleteCategory(Categoria categoria);
        bool Guardar();
    }
}
