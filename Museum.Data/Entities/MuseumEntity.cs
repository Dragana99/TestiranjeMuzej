using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Museum.Data.Entities
{
    [Table("Museum")]
    public class MuseumEntity
    {
        [Column("museumId")]
        public int Id { get; set; }

        [Column("museumName")]
        public string Name { get; set; }

        [Column("streetAddress")]
        public string Address { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("email")]
        public string Email { get; set; }
        
        [Column("phoneNumber")]
        public string Phone { get; set; }

        public virtual ICollection<AuditoriumEntity> Auditoriums { get; set; }
    }
}
