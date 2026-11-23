using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("usuarioInterno")]
    public class UsuarioInterno
    {
        [Key]
        [Column("id_usuarioInterno")]
        public int UsuarioInternoId { get; set; }

        [Required]
        [Column("id_pessoa")]
        public int PessoaId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nm_login")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("nm_senha")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [Column("st_situacao_acesso")]
        public string SituacaoAcesso { get; set; } = "ATIVO";

        [ForeignKey("PessoaId")]
        public Pessoa? Pessoa { get; set; }
    }
}
