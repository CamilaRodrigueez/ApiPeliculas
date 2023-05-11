using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using System.Net.Http;

namespace PeliculasWeb.Repository
{
    public class PeliculaRepository : Repository<Pelicula>, IPeliculaRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PeliculaRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
