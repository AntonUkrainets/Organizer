using Server.Constants;
using System;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ScheduleView;

namespace BusinessOrganizer.Extensions
{
    /// <summary>
    /// класс расширения события
    /// </summary>
    public static class AppointmentExtensions
    {
        /// <summary>
        /// преобразователь события из расписания в события доменной модели базы данных
        /// </summary>
        /// <param name="appointment">событие, которое нужно преобразовать</param>
        /// <returns></returns>
        public static Server.DataModel.Appointment ToDomainModel(
            this Appointment appointment)
        {
            //получение id выбранного приоритета
            var priorityId = appointment.Importance.GetPriorityId();

            //получение id выбранной категории
            var categoryId = appointment.Category.GetCategoryId();

            //получение id выбранного временного маркера
            var timeMarkerId = appointment.TimeMarker.GetTimeMarkerId();

            //получение id события для добавления его в базу данных
            var appointmentId = int.Parse(appointment.UniqueId);

            //преобразование из события расписания, в событие доменной модели базы данных
            return new Server.DataModel.Appointment()
            {
                Subject = appointment.Subject,
                Description = appointment.Body,
                StartTime = appointment.Start,
                EndTime = appointment.End,
                IsAllDayEvent = appointment.IsAllDayEvent,
                Id = appointmentId,
                PriorityId = priorityId,
                CategoryId = categoryId,
                TimeMarkerId = timeMarkerId,
            };
        }

        /// <summary>
        /// преобразователь осбытия из доменной модели базы данных в событие расписания
        /// </summary>
        /// <param name="appointment">событие, которое нужно преобразовать</param>
        /// <returns></returns>
        public static Appointment ToAppointment(
           this Server.DataModel.Appointment appointment)
        {
            //получение id выбранного приоритета
            var category = appointment.CategoryId.GetCategory();

            //получение id выбранной категории
            var priority = appointment.PriorityId.GetImportance();

            //получение id выбранного временного маркера
            var timeMarker = appointment.TimeMarkerId.GetTimeMarker();

            //преобразование в событие расписания из доменной модели базы данных
            return new Appointment()
            {
                Subject = appointment.Subject,
                Body = appointment.Description,
                Start = appointment.StartTime.Value,
                End = appointment.EndTime.Value,
                IsAllDayEvent = appointment.IsAllDayEvent.Value,
                UniqueId = appointment.Id.ToString(),
                Category = category,
                Importance = priority,
                TimeMarker = timeMarker,
            };
        }
    }
}