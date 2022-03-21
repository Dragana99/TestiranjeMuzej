using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Domain.Models
{
    public class ResponseModel<T>
    {
        public T DomainModel { get; set; }

        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }
    }
}
