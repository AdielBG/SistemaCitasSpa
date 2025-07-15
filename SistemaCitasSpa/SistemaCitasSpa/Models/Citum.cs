using System;
using System.Collections.Generic;

namespace SistemaCitasSpa.Models;

public partial class Citum
{
    public int CitaID { get; set; }

    public int PacienteID { get; set; }

    public int ServicioID { get; set; }

    public int TerapeutaID { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;

    public virtual Terapeutum Terapeuta { get; set; } = null!;
}
