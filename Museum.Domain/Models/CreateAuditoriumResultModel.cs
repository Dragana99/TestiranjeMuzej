using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class CreateAuditoriumResultModel
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public AuditoriumDomainModel Auditorium { get; set; }
    }
}
