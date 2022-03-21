using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Museum.API.Models
{
    public class CreateMuseumModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Nije moguce kreirati muzej")]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
