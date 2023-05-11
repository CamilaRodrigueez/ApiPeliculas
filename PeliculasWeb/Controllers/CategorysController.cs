using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    public class CategorysController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategorysController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #region Views
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Categoria() { });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            Categoria itemCategoria = new Categoria();

            if (id == null)
            {
                return NotFound();
            }

            itemCategoria = await _categoryRepository.GetAsync(Constants.RUTA_CATEGORIAS_API, id.GetValueOrDefault());

            if (itemCategoria == null)
            {
                return NotFound();
            }


            return View(itemCategoria);
        } 
        #endregion



        [HttpGet]
        public async Task<IActionResult> GetAllCategorys()
        {
            return Json(new { data = await _categoryRepository.GetAllAsync(Constants.RUTA_CATEGORIAS_API) });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.CreateAsync(Constants.RUTA_CATEGORIAS_API,categoria, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(Constants.RUTA_CATEGORIAS_API + categoria.Id, categoria, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _categoryRepository.DeleteAsync(Constants.RUTA_CATEGORIAS_API, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Registro eliminado correctamente!"});
            }

            return Json(new { success = true, message = "Hubo un error al eliminar el registro!" });
        }
    }
}
