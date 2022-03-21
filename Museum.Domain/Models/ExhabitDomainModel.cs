using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class ExhabitDomainModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public int Year { get; set; }
    }
}
