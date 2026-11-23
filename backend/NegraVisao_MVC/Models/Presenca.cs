using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("presenca")]
    public class Presenca
    {
        [Key]
        [Column("id_presenca")]
        public int PresencaId { get; set; }

        [Required]
        [Column("id_inscricao")]
        public int InscricaoId { get; set; }

        [Required]
        [Column("id_ocorrencia")]
        public int OcorrenciaId { get; set; }

        [Required]
        [Column("st_situacao_presenca")]
        public string SituacaoPresenca { get; set; } = "PRESENTE";

        [ForeignKey("InscricaoId")]
        public Inscricao? Inscricao { get; set; }

        [ForeignKey("OcorrenciaId")]
        public Ocorrencia? Ocorrencia { get; set; }
    }
}
