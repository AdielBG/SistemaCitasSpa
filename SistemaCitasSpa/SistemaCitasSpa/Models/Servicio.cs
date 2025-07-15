using System;
using System.Collections.Generic;

namespace SistemaCitasSpa.Models;

public partial class Servicio
{
    public int ServicioID { get; set; }

    public string NombreServicio { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int DuracionEnMinutos { get; set; }

    public decimal Precio { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
