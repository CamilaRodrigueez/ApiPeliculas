using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<AppUser> GetUsuarios();

        AppUser GetUsuario(string usuarioId);
        bool IsUniqueUser(string nombreUsuario);
        Task<UsuarioLoginResponseDto> Login(UsuarioLoginDto usuarioLoginDto );
        Task<UsuarioDatosDto> Register(UsuarioRegisterDto usuarioRegisterDto);
        //bool Guardar();
    }
}
