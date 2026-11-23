using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("responsavel")]
    public class Responsavel
    {
        [Key]
        [Column("id_responsavel")]
        public int ResponsavelId { get; set; }

        [Required]
        [Column("id_pessoa")]
        public int PessoaId { get; set; }

        [MaxLength(255)]
        [Column("ds_responsavel")]
        public string? DescricaoAtuacao { get; set; }

        [ForeignKey("PessoaId")]
        public Pessoa? Pessoa { get; set; }

        public ICollection<EventoResponsavel> EventoResponsaveis { get; set; } = new List<EventoResponsavel>();
    }
}
