using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataModel
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsAllDayEvent { get; set; }

        public int? PriorityId { get; set; }

        [ForeignKey(nameof(PriorityId))]
        public Priority Priority { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int? TimeMarkerId { get; set; }

        [ForeignKey(nameof(TimeMarkerId))]
        public TimeMarker TimeMarker { get; set; }
    }
}