using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        #region Attributes
        private readonly DataContext _dbcontext;
        #endregion Attributes
        public CategoriaRepository(DataContext dbcontext)
        {
           _dbcontext = dbcontext;
        }
        public bool CreateCategory(Categoria categoria)
        {
           categoria.FechaCrecion=DateTime.Now;
            _dbcontext.CategoriaEntity.Add(categoria);
            return Guardar();
        }

        public bool DeleteCategory(Categoria categoria)
        {
            _dbcontext.CategoriaEntity.Remove(categoria);
            return Guardar();
        }

        public bool ExistsCategory(string nombre)
        {
            bool valor = _dbcontext.CategoriaEntity.Any(x => x.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExistsCategory(int id)
        {
            return   _dbcontext.CategoriaEntity.Any(x => x.Id == id);
           
        }

        public Categoria GetCategoria(int idCategoria)
        {
          return  _dbcontext.CategoriaEntity.FirstOrDefault(x => x.Id == idCategoria);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _dbcontext.CategoriaEntity.OrderBy(x => x.Nombre).ToList();
        }

        public bool Guardar()
        {
           return _dbcontext.SaveChanges() >=0 ? true : false;

        }

        public bool UpdateCategory(Categoria categoria)
        {
            categoria.FechaCrecion = DateTime.Now;
            _dbcontext.CategoriaEntity.Update(categoria);
            return Guardar();
        }
    }
}
