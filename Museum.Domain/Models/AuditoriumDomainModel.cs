using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class AuditoriumDomainModel
    {
        public int Id { get; set; }

        public int MuseumId { get; set; }

        public string Name { get; set; }

    }
}
