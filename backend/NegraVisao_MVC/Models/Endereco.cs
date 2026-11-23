using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("endereco")]
    public class Endereco
    {
        [Key]
        [Column("id_endereco")]
        public int EnderecoId { get; set; }

        [Required]
        [Column("id_pessoa")]
        public int PessoaId { get; set; }

        [Required]
        [MaxLength(8)]
        [Column("nr_cep")]
        public string Cep { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("ds_logradouro")]
        public string Logradouro { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        [Column("nr_imovel")]
        public string NumeroImovel { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("ds_complemento")]
        public string? Complemento { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ds_bairro")]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("ds_municipio")]
        public string Municipio { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        [Column("sg_uf")]
        public string Uf { get; set; } = string.Empty;

        [ForeignKey("PessoaId")]
        public Pessoa? Pessoa { get; set; }
    }
}
