using System.ComponentModel.DataAnnotations;

namespace SistemaCitasSpa.Models;

public partial class Terapeutum
{
    public int TerapeutaID { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La especialidad del terapeuta es obligatoria.")]
    [StringLength(100, ErrorMessage = "La especialidad no puede tener más de 100 caracteres.")]
    public string? Especialidad { get; set; }


    [Required(ErrorMessage = "El telefono del terapeuta es obligatorio.")]
    [Phone(ErrorMessage = "El número de teléfono no es válido.")]
    [StringLength(15, ErrorMessage = "El número de teléfono no puede tener más de 15 caracteres.")]
    public string? Telefono { get; set; }


    [Required(ErrorMessage = "El correo del terapeuta es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede tener más de 100 caracteres.")]
    public string? Correo { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
