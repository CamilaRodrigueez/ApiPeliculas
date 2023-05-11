using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly IUsuarioRepository _usuariosRepository;

        public UsuariosController( IUsuarioRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        #region Views
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Usuario() { });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            Usuario itemUsuario = new Usuario();

            if (id == null)
            {
                return NotFound();
            }

            itemUsuario = await _usuariosRepository.GetAsync(Constants.RUTA_USUARIOS_API, id.GetValueOrDefault());

            if (itemUsuario == null)
            {
                return NotFound();
            }


            return View(itemUsuario);
        } 
        #endregion



        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Json(new { data = await _usuariosRepository.GetAllAsync(Constants.RUTA_USUARIOS_API) });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuariosRepository.CreateAsync(Constants.RUTA_USUARIOS_API,usuario, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuariosRepository.UpdateAsync(Constants.RUTA_USUARIOS_API + usuario.Id, usuario, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _usuariosRepository.DeleteAsync(Constants.RUTA_USUARIOS_API, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Registro eliminado correctamente!"});
            }

            return Json(new { success = true, message = "Hubo un error al eliminar el registro!" });
        }
    }
}
