using Microsoft.AspNetCore.Authentication.Cookies;
using PeliculasWeb.Repository;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Services;
using PeliculasWeb.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Se agrega esta línea para los llamdos HTTP
builder.Services.AddHttpClient();
#region Authentication --> 1
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath= "/Home/Login";
        options.AccessDeniedPath= "/Home/AccessDenied";
        options.SlidingExpiration = true;
    }
    );
#endregion
//Agregar repositorios creados 
#region Inyeccion de Dependecias
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); 
builder.Services.AddScoped<IAccountRepository, AccountRepository>();



//Inyecccion de dependencia del ContextAccessor @inject en el Layout
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion


#region Agregando Session--->> Authentication 2
builder.Services.AddSession(options =>
{
options.IdleTimeout = TimeSpan.FromMinutes(30);
options.Cookie.HttpOnly = true;
options.Cookie.IsEssential = true;
}); 
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Damos soporte para Cors
app.UseCors(x=> x
      .AllowAnyOrigin()
     .AllowAnyMethod() 
     .AllowAnyHeader()
    );

#region Authentication --->> 3
app.UseSession();
app.UseAuthentication(); 
#endregion
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
