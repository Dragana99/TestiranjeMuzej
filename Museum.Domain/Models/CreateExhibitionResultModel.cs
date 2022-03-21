using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class CreateExhibitionResultModel
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public ExhibitionDomainModel Exhibiition { get; set; } 
    }
}
