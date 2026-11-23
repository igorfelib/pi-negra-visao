using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("evento_responsavel")]
    public class EventoResponsavel
    {
        [Column("id_evento")]
        public int EventoId { get; set; }

        [Column("id_responsavel")]
        public int ResponsavelId { get; set; }

        [ForeignKey("EventoId")]
        public Evento? Evento { get; set; }

        [ForeignKey("ResponsavelId")]
        public Responsavel? Responsavel { get; set; }
    }
}
