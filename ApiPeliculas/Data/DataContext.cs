using ApiPeliculas.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class DataContext:IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Categoria> CategoriaEntity { get; set; }
        public DbSet<Pelicula> PeliculaEntity { get; set; }
        public DbSet<Usuario> UsuarioEntity { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
    }
}
