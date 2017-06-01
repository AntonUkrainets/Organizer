using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataModel
{
    public class TimeMarker
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public static implicit operator TimeMarker(int? v)
        {
            throw new NotImplementedException();
        }
    }
}