using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Services.Interface;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    public class PeliculasController : Controller
    {

        private readonly ICategoryServices _categoryServices;
        private readonly IPeliculaRepository _peliculaRepository;

        public PeliculasController(ICategoryServices categoryServices, IPeliculaRepository peliculaRepository)
        {
            _categoryServices = categoryServices;
            _peliculaRepository = peliculaRepository;
        }

        #region Views
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Pelicula() { });
        }

        [HttpGet]
        public async Task< IActionResult> Create()
        {
            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = await  _categoryServices.GetAllCategoriasSelectListItem(),
                Pelicula= new Pelicula()
            };
            return View(objVM);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = await _categoryServices.GetAllCategoriasSelectListItem(),
                Pelicula = new Pelicula()
            }; 

            if (id == null)
            {
                return NotFound();
            }

            objVM.Pelicula = await _peliculaRepository.GetAsync(Constants.RUTA_PELICULAS_API, id.GetValueOrDefault());

            if (objVM.Pelicula == null)
            {
                return NotFound();
            }


            return View(objVM);
        } 
        #endregion



        [HttpGet]
        public async Task<IActionResult> GetAllPeliculas()
        {
            return Json(new { data = await _peliculaRepository.GetAllAsync(Constants.RUTA_PELICULAS_API)});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula)
        {
            

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = await _categoryServices.GetAllCategoriasSelectListItem(),
                Pelicula = new Pelicula()
            };

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count>0)
                {
                    byte[] data = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            data = ms1.ToArray();
                        }
                    }

                    pelicula.RutaImagen = data;
                }
                else
                {
                    return View(objVM);
                }
                await _peliculaRepository.CreateAsync(Constants.RUTA_PELICULAS_API, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }
            

            return View(objVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] data = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            data = ms1.ToArray();
                        }
                    }

                    pelicula.RutaImagen = data;
                }
                else
                {
                    var peliculaFromDb = await _peliculaRepository.GetAsync(Constants.RUTA_PELICULAS_API, pelicula.Id);
                    pelicula.RutaImagen = peliculaFromDb.RutaImagen;
                }
                await _peliculaRepository.UpdateAsync(Constants.RUTA_PELICULAS_API+ pelicula.Id, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _peliculaRepository.DeleteAsync(Constants.RUTA_PELICULAS_API, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Registro eliminado correctamente!"});
            }

            return Json(new { success = true, message = "Hubo un error al eliminar el registro!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeliculasInCategory(int id)
        {
                return Json(new { data = await _peliculaRepository.GetAllPeliculasInCategory(Constants.RUTA_PELICULAS_EN_CATEGORIA_API,id) });  
        }

       
    }
}
