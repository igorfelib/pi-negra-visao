using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("pessoa_papel")]
    public class PessoaPapel
    {
        [Column("id_pessoa")]
        public int PessoaId { get; set; }

        [Column("id_papel")]
        public int PapelId { get; set; }

        [ForeignKey("PessoaId")]
        public Pessoa? Pessoa { get; set; }

        [ForeignKey("PapelId")]
        public Papel? Papel { get; set; }
    }
}
