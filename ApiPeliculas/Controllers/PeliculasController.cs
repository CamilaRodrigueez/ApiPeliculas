using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {

        private readonly IPeliculaRepository _pelrepo;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepository pelrepo, IMapper mapper)
        {
            _pelrepo = pelrepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas la películas
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelrepo.GetPeliculas();

            var listaPeliculasDto = new List<PeliculaDto>();

            foreach (var item in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(listaPeliculasDto);
        }
        /// <summary>
        /// Obtener una película por su Id
        /// </summary>
        /// <param name="peliculaId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var pelicula = _pelrepo.GetPelicula(peliculaId);

            if (pelicula == null)
            {
                return NotFound();
            }
            var peliculaDto = _mapper.Map<PeliculaDto>(pelicula);
            return Ok(peliculaDto);
        }
        /// <summary>
        /// Permite crear una película
        /// </summary>
        /// <param name="insertPeliculaDto"></param>
        /// <returns></returns>
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertPelicula([FromBody] PeliculaDto insertPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (insertPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_pelrepo.ExistsPelicula(insertPeliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoría ya existe");
                return StatusCode(404, ModelState);
            }
            var pelicula = _mapper.Map<Pelicula>(insertPeliculaDto);
            if (!_pelrepo.CreatePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }
        /// <summary>
        /// Permite Actualizar una película
        /// </summary>
        /// <param name="peliculaId"></param>
        /// <param name="peliculaDto"></param>
        /// <returns></returns>
       
        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaId:int}", Name = "UpdatePatPelicula")]
        [ProducesResponseType(204)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (peliculaDto == null || peliculaId != peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelrepo.UpdatePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
       
        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "DeletePelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeletePelicula(int peliculaId)
        {
            if (!_pelrepo.ExistsPelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _pelrepo.GetPelicula(peliculaId);
            if (!_pelrepo.DeletePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Permite filtrar películas en una categoría en específico
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetAllPeliculasInCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllPeliculasInCategory(int categoryId)
        {
            var listaPeliculas = _pelrepo.GetAllPeliculasInCategory(categoryId);
            if (listaPeliculas== null)
            {
                return NotFound();
            }
            var listaPeliculasDto = new List<PeliculaDto>();

            foreach (var item in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(listaPeliculasDto);
        }

        /// <summary>
        /// Obtener todas la películas
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("SearchPelicula")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SearchPelicula(string nombre)
        {
            try
            {
                var resultado = _pelrepo.SearchPelicula(nombre.Trim());
                if (resultado.Any())
                {
                    return Ok(resultado);

                }
                return NotFound();  
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,"Error recuperando datos");
            }
           
        }
    }
}
