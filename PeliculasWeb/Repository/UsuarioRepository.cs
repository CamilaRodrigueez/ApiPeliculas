using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using System.Net.Http;

namespace PeliculasWeb.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UsuarioRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
