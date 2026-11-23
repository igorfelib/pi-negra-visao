namespace NegraVisao_MVC.Models
{
    public class EventoPublicoViewModel
    {
        public int EventoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string TipoEvento { get; set; } = string.Empty;
        public string Local { get; set; } = string.Empty;
        public string Classificacao { get; set; } = string.Empty;
        public string? Frequencia { get; set; }
        public string DataExibicao { get; set; } = string.Empty;
        public string HorarioExibicao { get; set; } = string.Empty;
        public string MetaModal { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = "/img/capoeira.jpeg";
        public int VagasDisponiveis { get; set; }
        public uint Capacidade { get; set; }
        public bool InscricoesAbertas { get; set; }
        public string StatusInscricaoTexto { get; set; } = string.Empty;
    }

    public class InscricaoFormViewModel
    {
        public int? EventoId { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string DataNascimento { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string NumeroImovel { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;

        public List<EventoPublicoViewModel> EventosDisponiveis { get; set; } = new();
        public string? CodigoConfirmado { get; set; }
        public string? EventoConfirmadoTitulo { get; set; }
        public string? EventoConfirmadoMeta { get; set; }
        public string? MensagemErro { get; set; }
    }

    public class ResponsavelFormInput
    {
        public int? ResponsavelId { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? DescricaoAtuacao { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string NumeroImovel { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
    }

    public class EventoFormInput
    {
        public int? EventoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string TipoEvento { get; set; } = "OFICINA";
        public string Local { get; set; } = string.Empty;
        public int ResponsavelId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Classificacao { get; set; } = "UNICO";
        public string? Frequencia { get; set; }
        public string? ModalidadeDuracao { get; set; }
        public uint? QuantidadeTotalOcorrencias { get; set; }
        public DateTime? DataFinal { get; set; }
        public uint Capacidade { get; set; }
        public uint? LimiteFaltasConsecutivas { get; set; }
        public DateTime DataPrimeiraOcorrencia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public DateTime DataHoraAberturaInscricao { get; set; }
        public DateTime? DataHoraEncerramentoInscricao { get; set; }
    }

    public class AdminPainelViewModel
    {
        public List<Responsavel> Responsaveis { get; set; } = new();
        public List<Evento> Eventos { get; set; } = new();
        public List<Inscricao> Inscricoes { get; set; } = new();
        public string? MensagemSucesso { get; set; }
        public string? MensagemErro { get; set; }
        public string UsuarioLogado { get; set; } = string.Empty;
        public string PapelLogado { get; set; } = string.Empty;
    }
}
