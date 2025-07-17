using System;
using System.Collections.Generic;
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
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Correo inválido.")]
    public string? Correo { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? FechaNacimiento { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
