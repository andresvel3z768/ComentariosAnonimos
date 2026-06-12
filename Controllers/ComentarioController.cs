using ComentariosAnonimos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComentariosAnonimos.Controllers;

[Route("[controller]")]
public class ComentarioController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ComentarioController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        return View("~/Views/ComentarioView/Index.cshtml");
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var comentarios = await _context.Comentarios
            .OrderBy(c => c.Fecha)
            .ToListAsync();
        return Ok(comentarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var comentario = await _context.Comentarios.FindAsync(id);
        if (comentario == null)
            return NotFound(new { message = "Comentario no encontrado." });

        return Ok(comentario);
    }

    [HttpGet("mi-usuario")]
    public IActionResult MiUsuario()
    {
        var nombre = HttpContext.Items["NombreUsuario"] as string ?? "Anonimo";
        return Ok(new { nombreUsuario = nombre });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComentarioRequest request)
    {
        if (request == null || (string.IsNullOrWhiteSpace(request.Texto) && string.IsNullOrWhiteSpace(request.MediaUrl)))
            return BadRequest(new { message = "Escribe algo o adjunta una imagen." });

        if (request.ParentId.HasValue)
        {
            var existe = await _context.Comentarios.AnyAsync(c => c.Id == request.ParentId.Value);
            if (!existe)
                return BadRequest(new { message = "El comentario al que respondes no existe." });
        }

        var nombreUsuario = HttpContext.Items["NombreUsuario"] as string
            ?? NombreGenerator.Generar();

        var comentario = new Comentario
        {
            Texto = request.Texto ?? "",
            NombreUsuario = nombreUsuario,
            ParentId = request.ParentId,
            MediaUrl = string.IsNullOrWhiteSpace(request.MediaUrl) ? null : request.MediaUrl,
            Fecha = DateTime.UtcNow
        };

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Details), new { id = comentario.Id }, comentario);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Debes seleccionar un archivo." });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        if (!allowedExtensions.Contains(ext))
            return BadRequest(new { message = "Formato no permitido. Usa: jpg, png, gif, webp." });

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"/uploads/{fileName}";
        return Ok(new { url });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ComentarioRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Texto))
            return BadRequest(new { message = "El texto del comentario es requerido." });

        var existente = await _context.Comentarios.FindAsync(id);
        if (existente == null)
            return NotFound(new { message = "Comentario no encontrado." });

        existente.Texto = request.Texto;
        await _context.SaveChangesAsync();

        return Ok(existente);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existente = await _context.Comentarios.FindAsync(id);
        if (existente == null)
            return NotFound(new { message = "Comentario no encontrado." });

        _context.Comentarios.Remove(existente);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Comentario eliminado correctamente" });
    }
}

public class ComentarioRequest
{
    public string Texto { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public string? MediaUrl { get; set; }
}
