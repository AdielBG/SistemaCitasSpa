using System.ComponentModel.DataAnnotations;

namespace SistemaCitasSpa.Models;

public partial class Citum
{
    [Key]
    public int CitaID { get; set; }

    [Required(ErrorMessage = "El campo Paciente es obligatorio.")]
    [Display(Name = "Paciente")]
    public int PacienteID { get; set; }


    [Required(ErrorMessage = "El campo Servicio es obligatorio.")]
    [Display(Name = "Servicio")]
    public int ServicioID { get; set; }


    [Required(ErrorMessage = "El campo Terapeuta es obligatorio.")]
    [Display(Name = "Terapeuta")]
    public int TerapeutaID { get; set; }


    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de la cita")]
    public DateOnly Fecha { get; set; }


    [Required(ErrorMessage = "La hora es obligatoria.")]
    [DataType(DataType.Time)]
    [Display(Name = "Hora de la cita")]
    public TimeOnly Hora { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;

    public virtual Terapeutum Terapeuta { get; set; } = null!;
}
