using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Museum.Domain.Common;

namespace Museum.API.Models
{
    public class CreateAuditoriumModel
    {
        [Required]
        public int museumId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR)]
        public string auditName { get; set; }
    }
}
