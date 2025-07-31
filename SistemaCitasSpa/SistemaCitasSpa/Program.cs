using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


// Configurar la zona horaria para República Dominicana
var dominicanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");

// Configurar la cultura para República Dominicana
var culture = new CultureInfo("es-DO"); // Español - República Dominicana
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;


// Add services to the container.
builder.Services.AddControllersWithViews();



// Registrar el TimeZoneInfo como servicio para usar en los controladores
builder.Services.AddSingleton(dominicanTimeZone);


builder.Services.AddDbContext<SpaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SpaDB")));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.Use(async (context, next) =>
//{
//    await next();
//    // Evitar que el middleware cierre la conexión prematuramente
//    if (context.Response.HasStarted) return;
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
