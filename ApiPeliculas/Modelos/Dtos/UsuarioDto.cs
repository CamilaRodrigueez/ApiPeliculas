
namespace ApiPeliculas.Modelos.Dtos
{
    public class UsuarioDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
