using System;
using System.Collections.Generic;
using System.Text;
using Museum.Data.Entities;

namespace Museum.Domain.Models
{
    public class ExhibitionDomainModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Opening { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int AuditoriumId { get; set; }

        public Guid ExhabitId { get; set; }

        public string AuditoriumName { get; set; }

        public ExhabitEntity Exhabit { get; set; }
    }
}
