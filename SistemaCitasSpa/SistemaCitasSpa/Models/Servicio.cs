using System.ComponentModel.DataAnnotations;

namespace SistemaCitasSpa.Models;

public partial class Servicio
{
    public int ServicioID { get; set; }


    [Required(ErrorMessage = "El nombre del servicio es obligatorio.")]
    [StringLength(100)]
    public string NombreServicio { get; set; } = null!;


    [StringLength(250, ErrorMessage = "La descripción no debe exceder los 250 caracteres.")]
    [Required(ErrorMessage = "La descripcion del servicio es obligatoria.")]
    public string? Descripcion { get; set; }


    [Required(ErrorMessage = "La duración es obligatoria.")]
    [Range(1, 600, ErrorMessage = "La duración debe estar entre 1 y 600 minutos.")]
    public int DuracionEnMinutos { get; set; }


    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal Precio { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
