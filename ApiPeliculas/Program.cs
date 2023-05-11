using ApiPeliculas.Data;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using ApiPeliculas.PeliculasMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ApiPeliculas.Modelos;

var builder = WebApplication.CreateBuilder(args);
//Configuramos la conexi�n a SQLSERVER
builder.Services.AddDbContext<DataContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//Sporte para autenticaci�n con .NET Identity 
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();

//A�adimos Cach� 
builder.Services.AddResponseCaching();
//Agregamos los Repositorios creados
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");




//Agregar el AutoMapper

builder.Services.AddAutoMapper(typeof(PeliculaMapper));



//Aqu� se configura la autenticaci�n
builder.Services.AddAuthentication(x =>
{ 
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
  {
      x.RequireHttpsMetadata = false;
      x.SaveToken = true;
      x.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
          ValidateIssuer = false,
          ValidateAudience = false,              
      };
  });


builder.Services.AddControllers(options =>
{
    //Cach� profile. Un cache global
    options.CacheProfiles.Add("PorDefecto20Segundos", new CacheProfile() { Duration = 30 });

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description=
        "Autenticaci�n JWT usando el esquema Bearer. \r\n\r\n"+
        "Ingresa la palabra 'Bearer' seguida de un [espacio] y despu�s su token en el campo de abajo  \r\n\r\n" +
        "Ejemplo: \"Bearer fgdthfgjdffhybrvr\"",
        Name ="Authorization",
        In =ParameterLocation.Header,
        Scheme="Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference  = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id ="Bearer"
                             },
                Scheme="oauth2",
                Name = "Bearer",
                In =ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});


//Soporte para Cors 
//Se pueden habilitar:
//1-Un dominio,
//2-M�ltiples dominios,
//3-Cualquier dominio (Tener en cuenta seguirdad)
//Usamos de ejemplo el dominio: http://localhost:3223, se debe cambiar por el correcto
//Se usa (*) para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
})) ;
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/apiPeliculas/swagger/ApiPeliculasCategorias/swagger.json","API Categorias Pel�culas");
//        options.SwaggerEndpoint("/apiPeliculas/swagger/ApiPeliculas/swagger.json","API Pel�culas");
//        options.SwaggerEndpoint("/apiPeliculas/swagger/ApiPeliculasUsuarios/swagger.json","API Usuarios Pel�culas");
//        options.RoutePrefix = "";
//    });
//}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

//Soporte para Cors 
app.UseCors("PolicyCors");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
