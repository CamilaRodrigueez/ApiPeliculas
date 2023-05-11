using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utilidades;
using System.Diagnostics;
using System.Security.Claims;
using PeliculasWeb.Models.ViewModels;

namespace PeliculasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPeliculaRepository _peliculaRepository;

        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepository, ICategoryRepository categoryRepository, IPeliculaRepository peliculaRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _peliculaRepository= peliculaRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listaPeliculasYCategorias = new IndexVM()
            {
                ListaCategorias = await _categoryRepository.GetAllAsync(Constants.RUTA_CATEGORIAS_API),
                ListaPeliculas = await  _peliculaRepository.GetAllAsync(Constants.RUTA_PELICULAS_API),
            };


            return View(listaPeliculasYCategorias);
        }

        public async Task<IActionResult> IndexCategoria(int id)
        {
            var pelisEnCateogira = await _peliculaRepository.GetAllPeliculasInCategory(Constants.RUTA_PELICULAS_EN_CATEGORIA_API, id);
            return View(pelisEnCateogira);
        }

        public async Task<IActionResult> IndexBusqueda(string nombre)
        {
            var pelisEncontradas = await _peliculaRepository.SearchPelicula(Constants.RUTA_BUSCAR_PELICULAS_API,nombre);
            return View(pelisEncontradas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            UsuarioAccount usuario = new UsuarioAccount();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Login(UsuarioAccount usuarioLogion)
        {
            if (ModelState.IsValid)
            {
                UsuarioLoginResponse objeUser = await _accountRepository.LoguinAsync(Constants.RUTA_USUARIOS_API+ "loguin", usuarioLogion);
                if (objeUser?.result?.token == null)
                {
                    TempData["error"] = "Los datos son incorrectos";
                    return View();
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, objeUser.result.Usuario.userName));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("JWToken", objeUser.result.token);
                TempData["success"] = "Bienvenido/a " + objeUser.result.Usuario.userName;
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");   
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UsuarioRegister usuarioRegister)
        {
               bool result  = await _accountRepository.RegisterAsync(Constants.RUTA_USUARIOS_API + "registro", usuarioRegister);
               if (!result)
                {
                    return View();
                }
             TempData["success"] = "Registrado Exitosamente!";
             return RedirectToAction("Login");
        }

    }
}