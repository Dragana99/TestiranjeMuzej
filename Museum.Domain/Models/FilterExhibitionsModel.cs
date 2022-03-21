using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class FilterExhibitionsModel
    {
        public int MuseumId { get; set; }
        public int AuditoriumId { get; set; }
        public Guid? ExhabitId { get; set; }
        //public DateTime? StartTime { get; set; }
        //public DateTime? EndTime { get; set; }
    }
}
