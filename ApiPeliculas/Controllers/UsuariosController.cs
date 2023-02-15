using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        #region Attributes
        private readonly IUsuarioRepository _uRepo;
        private readonly IMapper _mapper;

        protected RespuestaAPI _respuestaApi;
        #endregion

        #region Builders
        public UsuariosController(IUsuarioRepository uRepo, IMapper mapper)
        {
            _uRepo = uRepo;
            _respuestaApi = new();
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _uRepo.GetUsuarios();

            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var item in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(item));
            }

            return Ok(listaUsuariosDto);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{usuarioId}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetUsuario(string usuarioId)
        {
            var usuario = _uRepo.GetUsuario(usuarioId);

            if (usuario == null)
            {
                return NotFound();
            }
            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
            return Ok(usuarioDto);
        }
        [AllowAnonymous]
        [HttpPost("registro")]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterDto usuarioRegisterDto)
        {
            bool validarNombreUsuarioUnico = _uRepo.IsUniqueUser(usuarioRegisterDto.NombreUsuario);

            if (!validarNombreUsuarioUnico)
            {
                _respuestaApi.StatusCode =HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");

                return BadRequest(_respuestaApi);
            }

            var usuario = await _uRepo.Register(usuarioRegisterDto);

            if (usuario == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error en el registro");

                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;

            return Ok(_respuestaApi);
        }
        [AllowAnonymous]
        [HttpPost("loguin")]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Loguin([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLoguin = await _uRepo.Login(usuarioLoginDto);
            if (respuestaLoguin.Usuario == null || string.IsNullOrEmpty(respuestaLoguin.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario o password son incorrectos");

                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuestaLoguin;

            return BadRequest(_respuestaApi);
        }


        #endregion
    }
}
