using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Museum.API.Models
{
    public class CreateExhibitionModel
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int AuditoriumId { get; set; }

        [Required]
        public Guid ExhabitId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Opening { get; set; }

        public string Description { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
