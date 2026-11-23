using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("papel")]
    public class Papel
    {
        [Key]
        [Column("id_papel")]
        public int PapelId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("nm_papel")]
        public string NomePapel { get; set; } = string.Empty;

        public ICollection<PessoaPapel> PessoaPapeis { get; set; } = new List<PessoaPapel>();
    }
}
