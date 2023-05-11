using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Usuario
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

    }
}
