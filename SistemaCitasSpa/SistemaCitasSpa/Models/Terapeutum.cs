namespace SistemaCitasSpa.Models;

public partial class Terapeutum
{
    public int TerapeutaID { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
