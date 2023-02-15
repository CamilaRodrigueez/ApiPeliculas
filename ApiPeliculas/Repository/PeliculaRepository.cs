using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository:IPeliculaRepository
    {
        #region Attributes
        private readonly DataContext _dbcontext;
        #endregion Attributes
        public PeliculaRepository(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public bool CreatePelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _dbcontext.PeliculaEntity.Add(pelicula);
            return Guardar();
        }

        public bool DeletePelicula(Pelicula pelicula)
        {
            _dbcontext.PeliculaEntity.Remove(pelicula);
            return Guardar();
        }

        public bool ExistsPelicula(string nombre)
        {
            bool valor = _dbcontext.PeliculaEntity.Any(x => x.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExistsPelicula(int id)
        {
            return _dbcontext.PeliculaEntity.Any(x => x.Id == id);

        }

        public ICollection<Pelicula> GetAllPeliculasInCategory(int categoriaId)
        {
            return _dbcontext.PeliculaEntity.Include(ca => ca.Categoria)
                .Where(ca=>ca.CategoriaId== categoriaId)
                .ToList();
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _dbcontext.PeliculaEntity.FirstOrDefault(x => x.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _dbcontext.PeliculaEntity.OrderBy(x => x.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _dbcontext.SaveChanges() >= 0 ? true : false;

        }

        public ICollection<Pelicula> SearchPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _dbcontext.PeliculaEntity;

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));
            }

            return query.ToList();
        }

        public bool UpdatePelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _dbcontext.PeliculaEntity.Update(pelicula);
            return Guardar();
        }
    }
}
