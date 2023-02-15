using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    //[Authorize(Roles ="admin")]También puedo agregar permiso a nivel de ROL
    [Authorize]
    [ApiController]
    //[Route("api/[controller]")] 1: Esta opción si cambio el nombre de controlador las personas conectadas a este enpoint pedería conexión
    [Route("api/categorys")]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoriaRepository _ctrepo;
        private readonly IMapper _mapper;

        public CategorysController(ICategoriaRepository ctrepo, IMapper mapper)
        {
            _ctrepo = ctrepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        //[ResponseCache(Duration =20)]
        [ResponseCache(CacheProfileName = "PorDefecto20Segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctrepo.GetCategorias();

            var listaCategoriasDto = new List<CategoryDto>();

            foreach (var item in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoryDto>(item));
            }

            return Ok(listaCategoriasDto);
        }
        [AllowAnonymous]
        [HttpGet("{idCategoria:int}", Name = "GetCategoria")]
        //[ResponseCache(Duration = 30)]
        [ResponseCache(CacheProfileName = "PorDefecto20Segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int idCategoria)
        {
            var categoria = _ctrepo.GetCategoria(idCategoria);

            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDto = _mapper.Map<CategoryDto>(categoria);
            return Ok(categoriaDto);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertCategory([FromBody] InsertCategoryDto insertCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (insertCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctrepo.ExistsCategory(insertCategoryDto.Nombre))
            {
                ModelState.AddModelError("", "La categoría ya existe");
                return StatusCode(404, ModelState);
            }
            var categoria = _mapper.Map<Categoria>(insertCategoryDto);
            if (!_ctrepo.CreateCategory(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { idCategoria = categoria.Id }, categoria);
        }
        
        [Authorize(Roles = "admin")]
        [HttpPatch("{idCategoria:int}", Name = "UpdateCategory")]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdatePatchCategory(int idCategoria, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoryDto == null || idCategoria != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoria = _mapper.Map<Categoria>(categoryDto);
            if (!_ctrepo.UpdateCategory(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{idCategoria:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteCategory(int idCategoria)
        {
            if (!_ctrepo.ExistsCategory(idCategoria))
            {
                return NotFound();
            }
            
            var categoria = _ctrepo.GetCategoria(idCategoria);
            if (!_ctrepo.DeleteCategory(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
