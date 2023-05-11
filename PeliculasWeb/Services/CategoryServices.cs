using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Services.Interface;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryServices;

        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryServices = categoryRepository;   
        }

        public async Task<IEnumerable<SelectListItem>> GetAllCategoriasSelectListItem()
        {
            IEnumerable<Categoria> listaCategoria = await _categoryServices.GetAllAsync(Constants.RUTA_CATEGORIAS_API);

            IEnumerable<SelectListItem> lista = null;

            lista = listaCategoria.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString(),
            });
            return lista;
        }
    }
}
