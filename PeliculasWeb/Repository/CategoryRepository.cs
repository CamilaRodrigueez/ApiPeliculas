using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utilidades;
using System.Net.Http;

namespace PeliculasWeb.Repository
{
    public class CategoryRepository : Repository<Categoria>, ICategoryRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
       
        

    }
}
