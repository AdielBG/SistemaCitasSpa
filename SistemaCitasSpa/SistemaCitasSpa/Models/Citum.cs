using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


    // ✅ CAMPOS CALCULADOS (NO MAPEADOS A LA BD)

    [NotMapped]
    [Display(Name = "Duración (minutos)")]
    public int Duracion => Servicio?.DuracionEnMinutos ?? 0;

    [NotMapped]
    [Display(Name = "Días restantes")]
    public int DiasRestantes
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Fecha.DayNumber - hoy.DayNumber;
        }
    }

    [NotMapped]
    [Display(Name = "Horas restantes")]
    public int HorasRestantes
    {
        get
        {
            var citaDateTime = Fecha.ToDateTime(Hora);
            var ahora = DateTime.Now;
            return (int)(citaDateTime - ahora).TotalHours;
        }
    }

    [NotMapped]
    [Display(Name = "Estado")]
    public string Estado
    {
        get
        {
            var citaDateTime = Fecha.ToDateTime(Hora);
            var ahora = DateTime.Now;

            if (citaDateTime > ahora)
                return "Vigente";
            else if (citaDateTime.Date == ahora.Date)
                return "En proceso";
            else
                return "Finalizado";
        }
    }
}
