using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using AutoMapper;
using System.Runtime;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculaMapper : Profile
    {
        public PeliculaMapper()
        {
            CreateMap<Categoria, CategoryDto>().ReverseMap();
            CreateMap<Categoria, InsertCategoryDto>().ReverseMap();
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            //CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<AppUser, UsuarioDatosDto>().ReverseMap();
            CreateMap<AppUser, UsuarioDto>().ReverseMap();
        }
    }

        
}
