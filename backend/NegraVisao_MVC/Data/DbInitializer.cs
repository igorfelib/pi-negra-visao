using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NegraVisao_MVC.Models;

namespace NegraVisao_MVC.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
        {
            try
            {
                var connectionString = configuration.GetConnectionString("MySqlConnection")
                    ?? "server=localhost;database=negra_visao;user=root;";

                // 1. Garante que o schema negra_visao exista antes de conectar no banco específico
                var builder = new MySqlConnectionStringBuilder(connectionString);
                var databaseName = string.IsNullOrWhiteSpace(builder.Database) ? "negra_visao" : builder.Database;
                builder.Database = "";
                builder.ConnectionTimeout = 2;

                await using (var serverConn = new MySqlConnection(builder.ConnectionString))
                {
                    await serverConn.OpenAsync();
                    await using var cmd = serverConn.CreateCommand();
                    cmd.CommandText = $"CREATE SCHEMA IF NOT EXISTS `{databaseName}` DEFAULT CHARACTER SET utf8;";
                    await cmd.ExecuteNonQueryAsync();
                }

                using var scope = services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // 2. Cria as 11 tabelas conforme o modelo caso ainda não existam
                await context.Database.EnsureCreatedAsync();

                // 3. Seed dos 4 Papéis de negócio (RN40)
                var papeisPadrao = new[] { "PARTICIPANTE", "RESPONSAVEL", "COLABORADOR", "ADMINISTRADOR" };
                foreach (var nomePapel in papeisPadrao)
                {
                    if (!await context.Papeis.AnyAsync(p => p.NomePapel == nomePapel))
                    {
                        context.Papeis.Add(new Papel { NomePapel = nomePapel });
                    }
                }
                await context.SaveChangesAsync();

                var papelAdmin = await context.Papeis.FirstAsync(p => p.NomePapel == "ADMINISTRADOR");
                var papelResp = await context.Papeis.FirstAsync(p => p.NomePapel == "RESPONSAVEL");

                // 4. Seed do Usuário Interno padrão (admin / admin) e Responsável padrão
                var adminUsuario = await context.UsuariosInternos
                    .Include(u => u.Pessoa)
                    .FirstOrDefaultAsync(u => u.Login == "admin");

                Pessoa pessoaAdmin;
                if (adminUsuario == null)
                {
                    pessoaAdmin = await context.Pessoas.FirstOrDefaultAsync(p => p.Cpf == "11144477735")
                        ?? new Pessoa
                        {
                            Nome = "Coordenação Negra Visão",
                            Cpf = "11144477735",
                            DataNascimento = new DateTime(1988, 11, 20),
                            Email = "contato@negravisao.org.br",
                            Telefone = "11999990000",
                            Situacao = "ATIVA"
                        };

                    if (pessoaAdmin.PessoaId == 0)
                    {
                        context.Pessoas.Add(pessoaAdmin);
                        await context.SaveChangesAsync();

                        context.Enderecos.Add(new Endereco
                        {
                            PessoaId = pessoaAdmin.PessoaId,
                            Cep = "12940000",
                            Logradouro = "Praça da Matriz",
                            NumeroImovel = "100",
                            Complemento = "Sede Cultural",
                            Bairro = "Centro",
                            Municipio = "Atibaia",
                            Uf = "SP"
                        });
                    }

                    if (!await context.PessoaPapeis.AnyAsync(pp => pp.PessoaId == pessoaAdmin.PessoaId && pp.PapelId == papelAdmin.PapelId))
                    {
                        context.PessoaPapeis.Add(new PessoaPapel
                        {
                            PessoaId = pessoaAdmin.PessoaId,
                            PapelId = papelAdmin.PapelId
                        });
                    }

                    context.UsuariosInternos.Add(new UsuarioInterno
                    {
                        PessoaId = pessoaAdmin.PessoaId,
                        Login = "admin",
                        Senha = "admin",
                        SituacaoAcesso = "ATIVO"
                    });

                    await context.SaveChangesAsync();
                }
                else
                {
                    pessoaAdmin = adminUsuario.Pessoa!;
                }

                var responsavelPadrao = await context.Responsaveis.FirstOrDefaultAsync(r => r.PessoaId == pessoaAdmin.PessoaId);
                if (responsavelPadrao == null)
                {
                    responsavelPadrao = new Responsavel
                    {
                        PessoaId = pessoaAdmin.PessoaId,
                        DescricaoAtuacao = "Coordenação Cultural e Pedagógica da Associação Negra Visão"
                    };
                    context.Responsaveis.Add(responsavelPadrao);

                    if (!await context.PessoaPapeis.AnyAsync(pp => pp.PessoaId == pessoaAdmin.PessoaId && pp.PapelId == papelResp.PapelId))
                    {
                        context.PessoaPapeis.Add(new PessoaPapel
                        {
                            PessoaId = pessoaAdmin.PessoaId,
                            PapelId = papelResp.PapelId
                        });
                    }
                    await context.SaveChangesAsync();
                }

                // 5. Seed dos 7 eventos culturais iniciais da Home caso não existam eventos no banco
                if (!await context.Eventos.AnyAsync())
                {
                    var agora = DateTime.Now;
                    var abertura = agora.Date.AddDays(-15);

                    var eventosIniciais = new List<(string Titulo, string Tipo, string Classif, string? Freq, string? Modalidade, uint? LimiteFaltas, DateTime Inicio, DateTime Fim, string Descricao)>
                    {
                        (
                            "CAPOEIRA",
                            "CURSO",
                            "RECORRENTE",
                            "SEMANAL",
                            "SEM_PREVISAO_DE_FIM",
                            3u,
                            new DateTime(agora.Year + 1, 6, 14, 19, 30, 0),
                            new DateTime(agora.Year + 1, 6, 14, 21, 0, 0),
                            "Vivência aberta com roda, música e movimento para celebrar a capoeira como prática de resistência, cultura e formação coletiva."
                        ),
                        (
                            "Colo de mãe",
                            "OFICINA",
                            "UNICO",
                            null,
                            null,
                            null,
                            new DateTime(agora.Year, 11, 29, 18, 30, 0),
                            new DateTime(agora.Year, 11, 29, 20, 30, 0),
                            "Encontro dedicado ao acolhimento, à escuta e às vivências afetivas que fortalecem a comunidade a partir do cuidado."
                        ),
                        (
                            "CINE VISÃO",
                            "PALESTRA",
                            "UNICO",
                            null,
                            null,
                            null,
                            new DateTime(agora.Year, 11, 6, 18, 30, 0),
                            new DateTime(agora.Year, 11, 6, 21, 0, 0),
                            "Sessão de cinema com conversa após a exibição, destacando narrativas negras e o poder do audiovisual como memória."
                        ),
                        (
                            "Dança: samba rock",
                            "OFICINA",
                            "UNICO",
                            null,
                            null,
                            null,
                            new DateTime(agora.Year, 11, 30, 14, 0, 0),
                            new DateTime(agora.Year, 11, 30, 16, 30, 0),
                            "Oficina e baile para aprender passos, experimentar ritmos e celebrar o samba rock como expressão de identidade."
                        ),
                        (
                            "Teatro: Ana Julia",
                            "OFICINA",
                            "UNICO",
                            null,
                            null,
                            null,
                            new DateTime(agora.Year + 1, 7, 12, 15, 0, 0),
                            new DateTime(agora.Year + 1, 7, 12, 17, 0, 0),
                            "Apresentação teatral com foco em afeto, denúncia e reflexão sobre experiências do cotidiano e da vivência negra."
                        ),
                        (
                            "QUINTA COLETIVA",
                            "PALESTRA",
                            "UNICO",
                            null,
                            null,
                            null,
                            new DateTime(agora.Year, 11, 13, 19, 0, 0),
                            new DateTime(agora.Year, 11, 13, 21, 0, 0),
                            "Roda de encontro para diálogo, criação conjunta e fortalecimento da participação comunitária por meio da arte."
                        ),
                        (
                            "Aya yoga",
                            "CURSO",
                            "RECORRENTE",
                            "SEMANAL",
                            "SEM_PREVISAO_DE_FIM",
                            3u,
                            new DateTime(agora.Year + 1, 10, 16, 19, 0, 0),
                            new DateTime(agora.Year + 1, 10, 16, 20, 30, 0),
                            "Prática guiada de yoga com foco em respiração, presença e bem-estar para corpo e mente em um ambiente acolhedor."
                        )
                    };

                    foreach (var item in eventosIniciais)
                    {
                        var ev = new Evento
                        {
                            Titulo = item.Titulo,
                            Descricao = item.Descricao,
                            TipoEvento = item.Tipo,
                            Local = "Sede Negra Visão - Atibaia/SP",
                            Classificacao = item.Classif,
                            Frequencia = item.Freq,
                            ModalidadeDuracao = item.Modalidade,
                            Capacidade = 25,
                            DataHoraAberturaInscricao = abertura,
                            DataHoraEncerramentoInscricao = item.Classif == "UNICO" ? item.Inicio.AddHours(-2) : null,
                            LimiteFaltasConsecutivas = item.LimiteFaltas,
                            EstadoEvento = "PROGRAMADO",
                            Visibilidade = "PUBLICADO"
                        };

                        context.Eventos.Add(ev);
                        await context.SaveChangesAsync();

                        context.EventoResponsaveis.Add(new EventoResponsavel
                        {
                            EventoId = ev.EventoId,
                            ResponsavelId = responsavelPadrao.ResponsavelId
                        });

                        context.Ocorrencias.Add(new Ocorrencia
                        {
                            EventoId = ev.EventoId,
                            DataHoraInicio = item.Inicio,
                            DataHoraFim = item.Fim,
                            Cancelada = false,
                            ListaPresencaFinalizada = false
                        });

                        if (item.Classif == "RECORRENTE")
                        {
                            context.Ocorrencias.Add(new Ocorrencia
                            {
                                EventoId = ev.EventoId,
                                DataHoraInicio = item.Inicio.AddDays(7),
                                DataHoraFim = item.Fim.AddDays(7),
                                Cancelada = false,
                                ListaPresencaFinalizada = false
                            });
                        }

                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Aviso ao inicializar o banco MySQL. Verifique se o servidor MySQL está em execução (localhost:3306).");
            }
        }
    }
}
