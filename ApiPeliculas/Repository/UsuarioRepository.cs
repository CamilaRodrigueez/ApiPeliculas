using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        #region Attributes
        private readonly DataContext _dbcontext;
        private string claveSecreta;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;



        #endregion Attributes
        #region Builder
        public UsuarioRepository(DataContext dbcontext, IConfiguration config, 
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _dbcontext = dbcontext;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        #endregion
        #region Methodos
        public AppUser GetUsuario(string usarioId)
        {

            return _dbcontext.AppUser.FirstOrDefault(u => u.Id == usarioId);
        }

        public ICollection<AppUser> GetUsuarios()
        {

            return _dbcontext.AppUser.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string nombreUsuario)
        {
            var usuarioBd = _dbcontext.AppUser.FirstOrDefault(u => u.UserName == nombreUsuario);

            if (usuarioBd == null)
            {
                return true;
            }
            
            return false;
        }

        //public async Task<Usuario> Register(UsuarioRegisterDto usuarioRegisterDto)
        //{
        //    var passwordEncritado = ObtenerMd5(usuarioRegisterDto.Password);

        //    Usuario usuario = new Usuario()
        //    {
        //        NombreUsuario = usuarioRegisterDto.NombreUsuario,
        //        Password = passwordEncritado,
        //        Nombre = usuarioRegisterDto.Nombre,
        //        Role = usuarioRegisterDto.Role

        //    };

        //    _dbcontext.Add(usuario);

        //    await _dbcontext.SaveChangesAsync();

        //    usuario.Password = passwordEncritado;

        //    return usuario;
        //}

        public async Task<UsuarioDatosDto> Register(UsuarioRegisterDto usuarioRegisterDto)
        {
            //var passwordEncritado = ObtenerMd5(usuarioRegisterDto.Password);

            AppUser usuario = new AppUser()
            {
                UserName = usuarioRegisterDto.NombreUsuario,
                Email = usuarioRegisterDto.NombreUsuario,
                NormalizedEmail=usuarioRegisterDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegisterDto.Nombre,
            };
            var result =  await _userManager.CreateAsync(usuario, usuarioRegisterDto.Password);

            if (result.Succeeded)
            {

                //Se debe entender solo para creación de lo Roles 
                //Solo la primera vez y es para crear los roles
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }
                //Se asigna rol al usuario
                await _userManager.AddToRoleAsync(usuario, "registrado");

                var usuarioRetornado = _dbcontext.AppUser.FirstOrDefault(u
                    => u.UserName == usuarioRegisterDto.NombreUsuario);

                //opcion 1
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado?.Nombre,
                //};
                //Opción 2

                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }
             return null;
        }

        public async Task<UsuarioLoginResponseDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passwordEncritado = ObtenerMd5(usuarioLoginDto.Password);

            var usuario = _dbcontext.AppUser.FirstOrDefault(
                                      u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                                      /*&& u.Password == passwordEncritado*/);

            bool isValida = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);
            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta.
            if (usuario == null || isValida== false)
            {
                return new UsuarioLoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }


            //Aquí existe el usuario entonces podemos procesar el Loguin 
            var roles = await _userManager.GetRolesAsync(usuario);


            var manejadorToken = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    //new Claim(ClaimTypes.Role, usuario.Role),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginResponseDto usuarioLoginResponseDto = new UsuarioLoginResponseDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)
            };

            return usuarioLoginResponseDto;

        }

        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso con en el Resgistro

        public static string ObtenerMd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }
        #endregion
    }
}
