using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Modelos
{
    public class AppUser:IdentityUser
    {
        //Añadir Campos personalizados 
        public string Nombre { get; set; }
    }
}
