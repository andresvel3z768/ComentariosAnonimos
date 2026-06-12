namespace ComentariosAnonimos.Models;

public class Comentario
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public string? MediaUrl { get; set; }
    public DateTime Fecha { get; set; }
}
