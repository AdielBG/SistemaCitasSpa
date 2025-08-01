using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SistemaCitasSpa.Utils;

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

    // Propiedades de navegación
    public virtual Paciente? Paciente { get; set; }
    public virtual Servicio? Servicio { get; set; }
    public virtual Terapeutum? Terapeuta { get; set; }

    // ✅ CAMPOS CALCULADOS (NO MAPEADOS A LA BD) - CORREGIDOS CON ZONA HORARIA
    [NotMapped]
    [Display(Name = "Duración (minutos)")]
    public int Duracion => Servicio?.DuracionEnMinutos ?? 0;

    [NotMapped]
    [Display(Name = "Días restantes")]
    public int DiasRestantes
    {
        get
        {
            var hoyDominicano = DateTimeUtils.GetDominicanToday();
            return Fecha.DayNumber - hoyDominicano.DayNumber;
        }
    }

    [NotMapped]
    [Display(Name = "Horas restantes")]
    public int HorasRestantes
    {
        get
        {
            var tiempoRestante = DateTimeUtils.GetTiempoRestante(Fecha, Hora);
            return (int)tiempoRestante.TotalHours;
        }
    }

    [NotMapped]
    [Display(Name = "Tiempo restante formateado")]
    public string TiempoRestanteFormateado => DateTimeUtils.FormatTiempoRestante(Fecha, Hora);

    [NotMapped]
    [Display(Name = "Estado")]
    public string Estado => DateTimeUtils.GetEstadoCita(Fecha, Hora);

    [NotMapped]
    [Display(Name = "Clase CSS del badge")]
    public string BadgeClass => Estado switch
    {
        "Vigente" => "info",
        "En proceso" => "warning",
        "Finalizada" => "secondary",
        _ => "secondary"
    };

    [NotMapped]
    [Display(Name = "Fecha formateada")]
    public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");

    [NotMapped]
    [Display(Name = "Hora formateada")]
    public string HoraFormateada => Hora.ToString("HH\\:mm");
}