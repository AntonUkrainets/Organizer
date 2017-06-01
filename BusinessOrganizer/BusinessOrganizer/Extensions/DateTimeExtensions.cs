using System;

namespace BusinessOrganizer.Extensions
{
    /// <summary>
    /// класс расширения DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// округление выбранного времени и увеличение его на минуту
        /// </summary>
        /// <param name="dt">время, которое необходимо округлить</param>
        /// <param name="d">время, на которое необходимо округлить</param>
        /// <returns></returns>
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            //вернуть округленное время
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        /// <summary>
        /// округление выбранного времени
        /// </summary>
        /// <param name="dt">время, которое необходимо округлить</param>
        /// <param name="d">время, на которое необходимо округлить</param>
        /// <returns></returns>
        public static DateTime Round(this DateTime dt, TimeSpan d)
        {
            //вернуть округленное время
            return new DateTime(dt.Ticks - dt.Ticks % d.Ticks);
        }
    }
}
