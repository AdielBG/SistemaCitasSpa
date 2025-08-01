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
    }
}