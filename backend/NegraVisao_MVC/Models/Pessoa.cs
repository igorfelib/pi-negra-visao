using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("pessoa")]
    public class Pessoa
    {
        [Key]
        [Column("id_pessoa")]
        public int PessoaId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nm_pessoa")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(11)]
        [Column("nr_cpf")]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [Column("dt_nascimento", TypeName = "date")]
        public DateTime DataNascimento { get; set; }

        [MaxLength(255)]
        [Column("ds_email")]
        public string? Email { get; set; }

        [MaxLength(11)]
        [Column("nr_telefone")]
        public string? Telefone { get; set; }

        [Required]
        [Column("st_pessoa")]
        public string Situacao { get; set; } = "ATIVA";

        public Endereco? Endereco { get; set; }
        public UsuarioInterno? UsuarioInterno { get; set; }
        public Responsavel? Responsavel { get; set; }
        public ICollection<PessoaPapel> PessoaPapeis { get; set; } = new List<PessoaPapel>();
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
    }
}
