using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Museum.Data.Entities
{
    [Table("Exhibition")]
    public class ExhibitionEntity
    {
        [Column("exhibitionId")]
        public Guid Id { get; set; }

        [Column("exhibitionName")]
        public string Name { get; set; }

        [Column("exhibitionTime")]
        public DateTime Opening { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("imagePath")]
        public string Image { get; set; }

        [Column("auditoriumId")]
        public int AuditoriumId { get; set; }

        [Column("exhabitId")]
        public Guid ExhabitId { get; set; }

        public virtual ExhabitEntity Exhabit { get; set; }

        public virtual AuditoriumEntity Auditorium { get; set; }

    }
}
