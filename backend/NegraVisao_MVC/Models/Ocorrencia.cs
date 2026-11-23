using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("ocorrencia")]
    public class Ocorrencia
    {
        [Key]
        [Column("id_ocorrencia")]
        public int OcorrenciaId { get; set; }

        [Required]
        [Column("id_evento")]
        public int EventoId { get; set; }

        [Required]
        [Column("dt_hora_inicio")]
        public DateTime DataHoraInicio { get; set; }

        [Required]
        [Column("dt_hora_fim")]
        public DateTime DataHoraFim { get; set; }

        [Column("st_cancelada")]
        public bool Cancelada { get; set; }

        [Column("st_lista_presenca_finalizada")]
        public bool ListaPresencaFinalizada { get; set; }

        [ForeignKey("EventoId")]
        public Evento? Evento { get; set; }

        public ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
    }
}
