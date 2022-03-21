using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Museum.Data.Entities
{
    [Table("Auditorium")]
    public class AuditoriumEntity
    {
        [Column("auditoriumId")]
        public int Id { get; set; }

        [Column("museumId")]
        public int MuseumId { get; set; }

        [Column("auditoriumName")]
        public string Name { get; set; }

        public virtual ICollection<ExhibitionEntity> Exhibition { get; set; }

        public virtual MuseumEntity Museum { get; set; }
    }
}
