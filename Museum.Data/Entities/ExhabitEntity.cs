using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Museum.Data.Entities
{
    [Table("Exhabit")]
    public class ExhabitEntity
    {
        [Column("exhabitId")]
        public Guid Id { get; set; }

        [Column("exhabitName")]
        public string Name { get; set; }

        [Column("picturePath")]
        public string Picture { get; set; }

        [Column("year")]
        public int Year { get; set; }

    }
}
