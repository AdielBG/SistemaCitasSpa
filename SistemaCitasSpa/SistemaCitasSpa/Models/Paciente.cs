using System;
using System.Collections.Generic;

namespace SistemaCitasSpa.Models;

public partial class Paciente
{
    public int PacienteID { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
