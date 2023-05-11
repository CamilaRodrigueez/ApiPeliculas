using PeliculasWeb.Models;

namespace PeliculasWeb.Repository.IRepository
{
    public interface IAccountRepository:IRepository<UsuarioAccount>
    {
        Task<UsuarioLoginResponse> LoguinAsync(string url, UsuarioAccount account);
        Task<bool> RegisterAsync(string url, UsuarioRegister account);
    }
}
