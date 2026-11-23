using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NegraVisao_MVC.Models
{
    [Table("evento")]
    public class Evento
    {
        [Key]
        [Column("id_evento")]
        public int EventoId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("ds_titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column("ds_evento", TypeName = "text")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column("tp_evento")]
        public string TipoEvento { get; set; } = "OFICINA";

        [Required]
        [MaxLength(255)]
        [Column("ds_local")]
        public string Local { get; set; } = string.Empty;

        [Required]
        [Column("st_classificacao")]
        public string Classificacao { get; set; } = "UNICO";

        [Column("st_frequencia")]
        public string? Frequencia { get; set; }

        [Column("ds_modalidade_duracao")]
        public string? ModalidadeDuracao { get; set; }

        [Column("nr_quantidade_total_ocorrencias")]
        public uint? QuantidadeTotalOcorrencias { get; set; }

        [Column("dt_final", TypeName = "date")]
        public DateTime? DataFinal { get; set; }

        [Required]
        [Column("nr_capacidade")]
        public uint Capacidade { get; set; }

        [Required]
        [Column("dt_hora_abertura_inscricao")]
        public DateTime DataHoraAberturaInscricao { get; set; }

        [Column("dt_hora_encerramento_inscricao")]
        public DateTime? DataHoraEncerramentoInscricao { get; set; }

        [Column("nr_limite_faltas_consecutivas")]
        public uint? LimiteFaltasConsecutivas { get; set; }

        [Required]
        [Column("st_estado_evento")]
        public string EstadoEvento { get; set; } = "PROGRAMADO";

        [Required]
        [Column("st_visibilidade")]
        public string Visibilidade { get; set; } = "OCULTO";

        [MaxLength(255)]
        [Column("ds_motivo_cancelamento")]
        public string? MotivoCancelamento { get; set; }

        public ICollection<EventoResponsavel> EventoResponsaveis { get; set; } = new List<EventoResponsavel>();
        public ICollection<Ocorrencia> Ocorrencias { get; set; } = new List<Ocorrencia>();
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
    }
}
