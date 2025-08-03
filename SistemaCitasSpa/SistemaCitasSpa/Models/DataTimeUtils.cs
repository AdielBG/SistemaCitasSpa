namespace SistemaCitasSpa.Utils
{
    public static class DateTimeUtils
    {
        // Zona horaria de República Dominicana (Atlantic Standard Time)
        private static readonly TimeZoneInfo DominicanTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");

        /// <summary>
        /// Obtiene la fecha y hora actual en República Dominicana
        /// </summary>
        public static DateTime GetDominicanNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DominicanTimeZone);
        }

        /// <summary>
        /// Obtiene solo la fecha actual en República Dominicana
        /// </summary>
        public static DateOnly GetDominicanToday()
        {
            return DateOnly.FromDateTime(GetDominicanNow());
        }

        /// <summary>
        /// Obtiene solo la hora actual en República Dominicana
        /// </summary>
        public static TimeOnly GetDominicanCurrentTime()
        {
            return TimeOnly.FromDateTime(GetDominicanNow());
        }

        /// <summary>
        /// Convierte una fecha/hora UTC a hora dominicana
        /// </summary>
        public static DateTime ConvertToDominicanTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, DominicanTimeZone);
        }

        /// <summary>
        /// Convierte una fecha/hora dominicana a UTC
        /// </summary>
        public static DateTime ConvertToUtc(DateTime dominicanDateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dominicanDateTime, DominicanTimeZone);
        }

        /// <summary>
        /// Combina DateOnly y TimeOnly en DateTime usando zona horaria dominicana
        /// </summary>
        public static DateTime CombineDateAndTime(DateOnly fecha, TimeOnly hora)
        {
            var localDateTime = fecha.ToDateTime(hora);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, DominicanTimeZone);
        }

        /// <summary>
        /// Verifica si una cita está vigente, en proceso o finalizada
        /// </summary>
        public static string GetEstadoCita(DateOnly fecha, TimeOnly hora)
        {
            var fechaHoraCita = fecha.ToDateTime(hora);
            var ahoraDominicana = GetDominicanNow();

            // Calcular diferencia en minutos
            var diferenciaMinutos = (fechaHoraCita - ahoraDominicana).TotalMinutes;

            if (diferenciaMinutos > 60) // Más de 1 hora en el futuro
                return "Vigente";
            else if (diferenciaMinutos >= 0 && diferenciaMinutos <= 60) // Entre ahora y 1 hora después
                return "En proceso";
            else // Ya pasó
                return "Finalizada";
        }

        /// <summary>
        /// Calcula el tiempo restante hasta una cita
        /// </summary>
        public static TimeSpan GetTiempoRestante(DateOnly fecha, TimeOnly hora)
        {
            var fechaHoraCita = fecha.ToDateTime(hora);
            var ahoraDominicana = GetDominicanNow();

            var tiempoRestante = fechaHoraCita - ahoraDominicana;
            return tiempoRestante.TotalMinutes > 0 ? tiempoRestante : TimeSpan.Zero;
        }

        /// <summary>
        /// Formatea el tiempo restante de manera legible
        /// </summary>
        public static string FormatTiempoRestante(DateOnly fecha, TimeOnly hora)
        {
            var tiempoRestante = GetTiempoRestante(fecha, hora);

            if (tiempoRestante == TimeSpan.Zero)
                return "Finalizada";

            if (tiempoRestante.TotalDays >= 1)
                return $"{tiempoRestante.Days} días, {tiempoRestante.Hours:D2}:{tiempoRestante.Minutes:D2}";
            else
                return $"{tiempoRestante.Hours:D2}:{tiempoRestante.Minutes:D2}";
        }

        /// <summary>
        /// Método de debugging para verificar cálculos
        /// </summary>
        public static string GetDebugInfo(DateOnly fecha, TimeOnly hora)
        {
            var fechaHoraCita = fecha.ToDateTime(hora);
            var ahoraDominicana = GetDominicanNow();
            var diferencia = fechaHoraCita - ahoraDominicana;

            return $"Cita: {fechaHoraCita:dd/MM/yyyy HH:mm} | " +
                   $"Ahora RD: {ahoraDominicana:dd/MM/yyyy HH:mm} | " +
                   $"Diferencia: {diferencia.TotalMinutes:F0} min | " +
                   $"Estado: {GetEstadoCita(fecha, hora)}";
        }

        /// <summary>
        /// Valida si una fecha y hora son válidas para agendar una cita
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidarFechaHoraCita(DateOnly fecha, TimeOnly hora)
        {
            var ahoraDominicana = GetDominicanNow();
            var fechaActualDominicana = GetDominicanToday();
            var horaActualDominicana = GetDominicanCurrentTime();

            // Validar que la fecha no sea anterior a hoy
            if (fecha < fechaActualDominicana)
            {
                return (false, "No se pueden agendar citas en fechas pasadas.");
            }

            // Si es hoy, validar que la hora no sea anterior a la actual + 30 minutos
            if (fecha == fechaActualDominicana)
            {
                var horaMinima = horaActualDominicana.AddMinutes(30);
                if (hora < horaMinima)
                {
                    return (false, $"Para citas del día de hoy, la hora debe ser al menos {horaMinima:HH\\:mm} (30 minutos de anticipación mínima).");
                }
            }

            // Validar horario de trabajo
            if (hora < new TimeOnly(7, 0) || hora > new TimeOnly(20, 0))
            {
                return (false, "Las citas solo se pueden agendar entre las 7:00 AM y 8:00 PM.");
            }

            // Validar que no sea domingo
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return (false, "No se pueden agendar citas los domingos.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Obtiene la próxima fecha y hora disponible para una cita
        /// </summary>
        public static (DateOnly Fecha, TimeOnly Hora) GetProximaFechaHoraDisponible()
        {
            var ahoraDominicana = GetDominicanNow();
            var fechaInicio = GetDominicanToday();
            var horaInicio = new TimeOnly(7, 0); // Hora de apertura

            // Si es hoy y aún hay tiempo
            if (fechaInicio == GetDominicanToday())
            {
                var horaMinima = GetDominicanCurrentTime().AddMinutes(30);
                if (horaMinima <= new TimeOnly(20, 0)) // Antes del cierre
                {
                    // Redondear a la próxima media hora
                    var minutos = horaMinima.Minute <= 30 ? 30 : 0;
                    var horasAdicionales = horaMinima.Minute > 30 ? 1 : 0;

                    return (fechaInicio, new TimeOnly(horaMinima.Hour + horasAdicionales, minutos));
                }
            }

            // Buscar el próximo día hábil
            var fechaProxima = fechaInicio.AddDays(1);
            while (fechaProxima.DayOfWeek == DayOfWeek.Sunday)
            {
                fechaProxima = fechaProxima.AddDays(1);
            }

            return (fechaProxima, horaInicio);
        }
    }
}