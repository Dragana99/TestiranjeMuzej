using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Museum.API.Models
{
    public class CreateExhabitModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Picture { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
