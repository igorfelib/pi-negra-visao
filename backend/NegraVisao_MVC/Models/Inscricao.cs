using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("inscricao")]
    public class Inscricao
    {
        [Key]
        [Column("id_inscricao")]
        public int InscricaoId { get; set; }

        [Required]
        [Column("id_pessoa")]
        public int PessoaId { get; set; }

        [Required]
        [Column("id_evento")]
        public int EventoId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("cd_inscricao")]
        public string CodigoInscricao { get; set; } = string.Empty;

        [Required]
        [Column("st_estado")]
        public string Estado { get; set; } = "CONFIRMADA";

        [MaxLength(255)]
        [Column("ds_motivo_cancelamento")]
        public string? MotivoCancelamento { get; set; }

        [Column("nr_faltas_consecutivas")]
        public uint FaltasConsecutivas { get; set; }

        [ForeignKey("PessoaId")]
        public Pessoa? Pessoa { get; set; }

        [ForeignKey("EventoId")]
        public Evento? Evento { get; set; }

        public ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
    }
}
