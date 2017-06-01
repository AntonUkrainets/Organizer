using Server.DataModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Класс для работы с базой данных
    /// </summary>
    public static class OrganizerRepository
    {
        /// <summary>
        /// получение коллекции событий из базы данных
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Appointment> GetAppointmens()
        {
            try
            {
                //подключение к базе данных
                using (OrganizerContext context = new OrganizerContext())
                {
                    //вернуть коллекцию событий
                    return new ObservableCollection<Appointment>(context.Appointments);
                }
            }
            catch
            {
                MessageBox.Show("База данных не была подключена");
            }

            return null;
        }

        /// <summary>
        /// получение повторений из базы данных
        /// </summary>
        /// <param name="appointmentId">id, по которому нужно получить повторения </param>
        /// <returns></returns>
        public static Recurrence GetRecurrence(int appointmentId)
        {
            //подключение к базе данных
            using (OrganizerContext context = new OrganizerContext())
            {        
                //сравнения id события с каждым повторением в базе данных и получение его
                var recurrence = context.Recurrences
                    .FirstOrDefault(x => x.AppointmentId == appointmentId);

                //вернуть полученное событие
                return recurrence;
            }
        }

        /// <summary>
        /// сохранение события и его повторения в базу данных
        /// </summary>
        /// <param name="appointment"></param>
        /// <param name="recurrence"></param>
        public static void Save(Appointment appointment, Recurrence recurrence = null)
        {
            //подключение к базе данных
            using (OrganizerContext context = new OrganizerContext())
            {
                //добавление события в базу данных
                context.Appointments.Add(appointment);

                //если повторение было создано
                if (recurrence != null)
                {
                    //присвоить повторению событие
                    recurrence.Appointment = appointment;

                    //и добавить его в коллекцию событий
                    context.Recurrences.Add(recurrence);
                }

                //сохранить все изменения в базе данных
                context.SaveChanges();
            }
        }

        /// <summary>
        /// метод изменения данных в базе данных
        /// </summary>
        /// <param name="changedAppointment">событие, которое следует изменить</param>
        /// <param name="recurrence">повторение, которое следует изменить</param>
        public static void Change(Appointment changedAppointment, Recurrence recurrence = null)
        {
            //подключение к базе данных
            using (OrganizerContext context = new OrganizerContext())
            {
                //обновление события в базе данных
                context.Appointments.Attach(changedAppointment);
                context.Entry(changedAppointment).State = System.Data.Entity.EntityState.Modified;

                //если повторение было создано
                if (recurrence != null)
                {
                    //обновить повторения в базе данных
                    context.Recurrences.Attach(recurrence);
                    context.Entry(recurrence).State = System.Data.Entity.EntityState.Modified;
                }

                //сохранить все изменения в базе данных
                context.SaveChanges();
            }
        }

        /// <summary>
        /// удаления события и его повторения в базе данных
        /// </summary>
        /// <param name="appointmentId">id, по которому нужно удалить событие</param>
        public static void Delete(int appointmentId)
        {
            //подключение к базе данных
            using (OrganizerContext context = new OrganizerContext())
            {
                //сравневние id события с каждым из них в таблице событий в базе данных, и удаление его
                context.Appointments.Remove(
                    context.Appointments.First(appointment => appointment.Id == appointmentId));

                //сохранить все изменения в базе данных
                context.SaveChanges();
            }
        }
    }
}