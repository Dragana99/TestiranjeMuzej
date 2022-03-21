using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class CreateMuseumResultModel
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public MuseumDomainModel Museum { get; set; }
    }
}
