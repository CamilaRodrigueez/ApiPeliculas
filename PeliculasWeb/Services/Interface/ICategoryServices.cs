using Microsoft.AspNetCore.Mvc.Rendering;

namespace PeliculasWeb.Services.Interface
{
    public interface ICategoryServices
    {
        Task<IEnumerable<SelectListItem>> GetAllCategoriasSelectListItem();
    }
}
