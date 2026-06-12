using ComentariosAnonimos.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Neon")));

var app = builder.Build();

// Middleware de privacidad: anula la IP remota para que no quede registrada
app.Use(async (context, next) =>
{
    context.Connection.RemoteIpAddress = null;
    await next();
});

// Middleware de identidad anonima persistente con cookie cifrada
app.Use(async (context, next) =>
{
    var protector = context.RequestServices
        .GetRequiredService<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider>()
        .CreateProtector("ComentariosAnonimos.Identity");

    const string cookieName = "anon_user";

    if (context.Request.Cookies.TryGetValue(cookieName, out var encrypted))
    {
        try
        {
            var bytes = Convert.FromBase64String(encrypted);
            var nombre = System.Text.Encoding.UTF8.GetString(protector.Unprotect(bytes));
            context.Items["NombreUsuario"] = nombre;
        }
        catch
        {
            encrypted = null;
        }
    }

    if (context.Items["NombreUsuario"] == null)
    {
        var nuevoNombre = NombreGenerator.Generar();
        context.Items["NombreUsuario"] = nuevoNombre;
        var encryptedBytes = protector.Protect(System.Text.Encoding.UTF8.GetBytes(nuevoNombre));
        context.Response.Cookies.Append(cookieName, Convert.ToBase64String(encryptedBytes), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            MaxAge = TimeSpan.FromDays(30)
        });
    }

    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Comentario");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Comentario}/{action=Index}")
    .WithStaticAssets();

app.MapGet("/", () => Results.Redirect("/Comentario"));

app.Run();
