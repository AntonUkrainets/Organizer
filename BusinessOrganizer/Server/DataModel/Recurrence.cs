using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataModel
{
    public class Recurrence
    {
        [Key]
        public Guid Id { get; set; }

        public int AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; }

        [MaxLength(200)]
        public string DaysOfWeekMask { get; set; }

        [MaxLength(200)]
        public string FirstDayOfWeek { get; set; }

        [MaxLength(200)]
        public string Frequency { get; set; }

        public DateTime? RecursUntil { get; set; }

        public int Interval { get; set; }

        public int? DayOrdinal { get; set; }
        public int? MaxOccurrences { get; set; }
        public int? MonthOfYear { get; set; }

        [MaxLength(200)]
        public string DaysOfMonth { get; set; }

        [MaxLength(200)]
        public string HoursOfDay { get; set; }

        [MaxLength(200)]
        public string MinutesOfHour { get; set; }
    }
}