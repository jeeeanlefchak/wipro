using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace winpro.services.Model
{

    [Table("coin")]
    public class Coin : EntityBase
    {
        [Column("moeda")]
        [Required]
        public string Moeda { get; set; }

        [Column("data_inicio")]
        [Required]
        public DateTime Data_Inicio { get; set; }

        [Column("data_fim")]
        [Required]
        public DateTime Data_Fim { get; set; }
    }
}
