using System.ComponentModel.DataAnnotations;

namespace SistemaCitasSpa.Models;

public partial class Paciente
{

    public int PacienteID { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string Apellido { get; set; } = null!;

    [Phone(ErrorMessage = "Teléfono inválido.")]
    [Required(ErrorMessage = "El telefono es obligatorio.")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Correo inválido.")]
    [Required(ErrorMessage = "El Correo es obligatorio.")]
    public string? Correo { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly? FechaNacimiento { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
