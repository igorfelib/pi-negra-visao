using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NegraVisao_MVC.Data;
using NegraVisao_MVC.Helpers;
using NegraVisao_MVC.Models;

namespace NegraVisao_MVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await MontarPainelViewModelAsync();
            vm.MensagemSucesso = TempData["MensagemSucesso"] as string;
            vm.MensagemErro = TempData["MensagemErro"] as string;
            return View(vm);
        }

        // Busca dados completos por CPF no painel interno (equipe interna visualiza dados completos)
        [HttpGet]
        public async Task<IActionResult> BuscarPessoaPorCpf(string? cpf)
        {
            var cpfLimpo = ValidacaoHelper.SomenteDigitos(cpf);
            if (!ValidacaoHelper.ValidarCpf(cpfLimpo))
                return Json(new { encontrado = false });

            var pessoa = await _context.Pessoas
                .Include(p => p.Endereco)
                .Include(p => p.Responsavel)
                .FirstOrDefaultAsync(p => p.Cpf == cpfLimpo);

            if (pessoa == null)
                return Json(new { encontrado = false });

            return Json(new
            {
                encontrado = true,
                dados = new
                {
                    responsavelId = pessoa.Responsavel?.ResponsavelId,
                    nome = pessoa.Nome,
                    dataNascimento = pessoa.DataNascimento.ToString("yyyy-MM-dd"),
                    telefone = pessoa.Telefone ?? "",
                    email = pessoa.Email ?? "",
                    descricaoAtuacao = pessoa.Responsavel?.DescricaoAtuacao ?? "",
                    cep = pessoa.Endereco?.Cep ?? "",
                    logradouro = pessoa.Endereco?.Logradouro ?? "",
                    numeroImovel = pessoa.Endereco?.NumeroImovel ?? "",
                    complemento = pessoa.Endereco?.Complemento ?? "",
                    bairro = pessoa.Endereco?.Bairro ?? "",
                    municipio = pessoa.Endereco?.Municipio ?? "",
                    uf = pessoa.Endereco?.Uf ?? ""
                }
            });
        }

        // ==========================================
        // 1. CRUD DE RESPONSÁVEIS (RN06, RN23-RN34, RN40)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarResponsavel(ResponsavelFormInput input)
        {
            var cpfLimpo = ValidacaoHelper.SomenteDigitos(input.Cpf);
            if (!ValidacaoHelper.ValidarCpf(cpfLimpo))
            {
                TempData["MensagemErro"] = "CPF inválido. Informe 11 dígitos válidos (RN24).";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(input.Nome) || input.DataNascimento == default || input.DataNascimento.Date > DateTime.Today)
            {
                TempData["MensagemErro"] = "Nome completo e Data de Nascimento válida (não futura) são obrigatórios (RN23/RN29).";
                return RedirectToAction(nameof(Index));
            }

            var cepLimpo = ValidacaoHelper.SomenteDigitos(input.Cep);
            var ufLimpa = (input.Uf ?? string.Empty).Trim().ToUpperInvariant();
            if (cepLimpo.Length != 8 ||
                string.IsNullOrWhiteSpace(input.Logradouro) ||
                string.IsNullOrWhiteSpace(input.NumeroImovel) ||
                string.IsNullOrWhiteSpace(input.Bairro) ||
                string.IsNullOrWhiteSpace(input.Municipio) ||
                ufLimpa.Length != 2)
            {
                TempData["MensagemErro"] = "Preencha o endereço completo do responsável (CEP de 8 dígitos, Logradouro, Número ou S/N, Bairro, Município e UF) (RN23/RN32).";
                return RedirectToAction(nameof(Index));
            }

            var emailFinal = string.IsNullOrWhiteSpace(input.Email) ? null : input.Email.Trim();
            if (emailFinal != null && !Regex.IsMatch(emailFinal, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                TempData["MensagemErro"] = "O e-mail informado possui formato inválido (RN33).";
                return RedirectToAction(nameof(Index));
            }

            var telFinal = string.IsNullOrWhiteSpace(input.Telefone) ? null : ValidacaoHelper.SomenteDigitos(input.Telefone);
            if (telFinal != null && (telFinal.Length < 10 || telFinal.Length > 11))
            {
                TempData["MensagemErro"] = "Quando informado, o telefone deve conter DDD e 10 ou 11 dígitos (RN34).";
                return RedirectToAction(nameof(Index));
            }

            // Reutiliza Pessoa existente pelo CPF (RN25, RN40)
            Pessoa? pessoa = null;
            if (input.ResponsavelId.HasValue && input.ResponsavelId.Value > 0)
            {
                var respEdicao = await _context.Responsaveis
                    .Include(r => r.Pessoa)
                        .ThenInclude(p => p!.Endereco)
                    .FirstOrDefaultAsync(r => r.ResponsavelId == input.ResponsavelId.Value);

                if (respEdicao != null)
                {
                    var conflitoCpf = await _context.Pessoas.AnyAsync(p => p.Cpf == cpfLimpo && p.PessoaId != respEdicao.PessoaId);
                    if (conflitoCpf)
                    {
                        TempData["MensagemErro"] = "O CPF informado já pertence a outra pessoa cadastrada no sistema (RN25).";
                        return RedirectToAction(nameof(Index));
                    }
                    pessoa = respEdicao.Pessoa;
                }
            }

            pessoa ??= await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Cpf == cpfLimpo);

            if (pessoa == null)
            {
                pessoa = new Pessoa
                {
                    Cpf = cpfLimpo,
                    Nome = input.Nome.Trim(),
                    DataNascimento = input.DataNascimento.Date,
                    Email = emailFinal,
                    Telefone = telFinal,
                    Situacao = "ATIVA"
                };
                _context.Pessoas.Add(pessoa);
                await _context.SaveChangesAsync();

                _context.Enderecos.Add(new Endereco
                {
                    PessoaId = pessoa.PessoaId,
                    Cep = cepLimpo,
                    Logradouro = input.Logradouro.Trim(),
                    NumeroImovel = input.NumeroImovel.Trim(),
                    Complemento = string.IsNullOrWhiteSpace(input.Complemento) ? null : input.Complemento.Trim(),
                    Bairro = input.Bairro.Trim(),
                    Municipio = input.Municipio.Trim(),
                    Uf = ufLimpa
                });
            }
            else
            {
                pessoa.Cpf = cpfLimpo;
                pessoa.Nome = input.Nome.Trim();
                pessoa.DataNascimento = input.DataNascimento.Date;
                pessoa.Email = emailFinal;
                pessoa.Telefone = telFinal;

                if (pessoa.Endereco == null)
                {
                    _context.Enderecos.Add(new Endereco
                    {
                        PessoaId = pessoa.PessoaId,
                        Cep = cepLimpo,
                        Logradouro = input.Logradouro.Trim(),
                        NumeroImovel = input.NumeroImovel.Trim(),
                        Complemento = string.IsNullOrWhiteSpace(input.Complemento) ? null : input.Complemento.Trim(),
                        Bairro = input.Bairro.Trim(),
                        Municipio = input.Municipio.Trim(),
                        Uf = ufLimpa
                    });
                }
                else
                {
                    pessoa.Endereco.Cep = cepLimpo;
                    pessoa.Endereco.Logradouro = input.Logradouro.Trim();
                    pessoa.Endereco.NumeroImovel = input.NumeroImovel.Trim();
                    pessoa.Endereco.Complemento = string.IsNullOrWhiteSpace(input.Complemento) ? null : input.Complemento.Trim();
                    pessoa.Endereco.Bairro = input.Bairro.Trim();
                    pessoa.Endereco.Municipio = input.Municipio.Trim();
                    pessoa.Endereco.Uf = ufLimpa;
                }
            }

            // Garante papel RESPONSAVEL na tabela pessoa_papel (RN40)
            var papelResp = await _context.Papeis.FirstOrDefaultAsync(p => p.NomePapel == "RESPONSAVEL");
            if (papelResp == null)
            {
                papelResp = new Papel { NomePapel = "RESPONSAVEL" };
                _context.Papeis.Add(papelResp);
                await _context.SaveChangesAsync();
            }

            if (!await _context.PessoaPapeis.AnyAsync(pp => pp.PessoaId == pessoa.PessoaId && pp.PapelId == papelResp.PapelId))
            {
                _context.PessoaPapeis.Add(new PessoaPapel
                {
                    PessoaId = pessoa.PessoaId,
                    PapelId = papelResp.PapelId
                });
            }

            var responsavel = await _context.Responsaveis.FirstOrDefaultAsync(r => r.PessoaId == pessoa.PessoaId);
            var descAtuacao = string.IsNullOrWhiteSpace(input.DescricaoAtuacao) ? null : input.DescricaoAtuacao.Trim();

            if (responsavel == null)
            {
                _context.Responsaveis.Add(new Responsavel
                {
                    PessoaId = pessoa.PessoaId,
                    DescricaoAtuacao = descAtuacao
                });
                TempData["MensagemSucesso"] = "Responsável cadastrado com sucesso!";
            }
            else
            {
                responsavel.DescricaoAtuacao = descAtuacao;
                TempData["MensagemSucesso"] = "Dados do responsável atualizados com sucesso!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirResponsavel(int id)
        {
            var resp = await _context.Responsaveis
                .Include(r => r.EventoResponsaveis)
                .FirstOrDefaultAsync(r => r.ResponsavelId == id);

            if (resp == null)
            {
                TempData["MensagemErro"] = "Responsável não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (resp.EventoResponsaveis.Any())
            {
                TempData["MensagemErro"] = "Não é possível excluir um responsável que já está vinculado a eventos cadastrados (RN06).";
                return RedirectToAction(nameof(Index));
            }

            var papelResp = await _context.Papeis.FirstOrDefaultAsync(p => p.NomePapel == "RESPONSAVEL");
            if (papelResp != null)
            {
                var vinculoPapel = await _context.PessoaPapeis
                    .FirstOrDefaultAsync(pp => pp.PessoaId == resp.PessoaId && pp.PapelId == papelResp.PapelId);
                if (vinculoPapel != null)
                    _context.PessoaPapeis.Remove(vinculoPapel);
            }

            _context.Responsaveis.Remove(resp);
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = "Responsável removido com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 2. CRUD E AÇÕES DE EVENTOS (RN01-RN09, RN13, RN16, RN41-RN44, RN48)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarEvento(EventoFormInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Titulo) ||
                string.IsNullOrWhiteSpace(input.Descricao) ||
                string.IsNullOrWhiteSpace(input.Local) ||
                input.ResponsavelId <= 0 ||
                input.Capacidade == 0)
            {
                TempData["MensagemErro"] = "Preencha Título, Tipo, Local, Responsável, Descrição e Capacidade positiva (RN06/RN13).";
                return RedirectToAction(nameof(Index));
            }

            var inicioPrimeira = input.DataPrimeiraOcorrencia.Date.Add(input.HoraInicio);
            var fimPrimeira = input.DataPrimeiraOcorrencia.Date.Add(input.HoraFim);

            if (fimPrimeira <= inicioPrimeira)
            {
                TempData["MensagemErro"] = "O horário de término da ocorrência deve ser posterior ao horário de início (RN02).";
                return RedirectToAction(nameof(Index));
            }

            string classificacao = input.Classificacao == "RECORRENTE" ? "RECORRENTE" : "UNICO";
            string? frequencia = null;
            string? modalidade = null;
            uint? qtdTotal = null;
            DateTime? dataFinal = null;
            uint? limiteFaltas = null;

            if (classificacao == "RECORRENTE")
            {
                if (string.IsNullOrWhiteSpace(input.Frequencia) || string.IsNullOrWhiteSpace(input.ModalidadeDuracao))
                {
                    TempData["MensagemErro"] = "Eventos recorrentes exigem Frequência e Modalidade de Duração (RN03/RN04).";
                    return RedirectToAction(nameof(Index));
                }

                frequencia = input.Frequencia;
                modalidade = input.ModalidadeDuracao;

                if (modalidade == "QUANTIDADE_OCORRENCIAS")
                {
                    if (!input.QuantidadeTotalOcorrencias.HasValue || input.QuantidadeTotalOcorrencias.Value < 2)
                    {
                        TempData["MensagemErro"] = "A modalidade por quantidade exige pelo menos 2 ocorrências (RN01/RN04).";
                        return RedirectToAction(nameof(Index));
                    }
                    qtdTotal = input.QuantidadeTotalOcorrencias.Value;
                }
                else if (modalidade == "DATA_FINAL")
                {
                    if (!input.DataFinal.HasValue || input.DataFinal.Value.Date <= input.DataPrimeiraOcorrencia.Date)
                    {
                        TempData["MensagemErro"] = "A data final da série recorrente deve ser posterior à primeira ocorrência e permitir ao menos 2 encontros (RN04).";
                        return RedirectToAction(nameof(Index));
                    }
                    dataFinal = input.DataFinal.Value.Date;
                }
                else if (modalidade == "SEM_PREVISAO_DE_FIM")
                {
                    if (!input.LimiteFaltasConsecutivas.HasValue || input.LimiteFaltasConsecutivas.Value == 0)
                    {
                        TempData["MensagemErro"] = "Eventos recorrentes sem previsão de fim exigem um Limite de Faltas Consecutivas inteiro positivo (RN48).";
                        return RedirectToAction(nameof(Index));
                    }
                    limiteFaltas = input.LimiteFaltasConsecutivas.Value;
                }
            }

            // RN16: Validação da janela de inscrição
            bool ehFinito = classificacao == "UNICO" || modalidade != "SEM_PREVISAO_DE_FIM";
            if (ehFinito)
            {
                if (!input.DataHoraEncerramentoInscricao.HasValue)
                {
                    TempData["MensagemErro"] = "Eventos finitos exigem data/hora de encerramento das inscrições (RN16).";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (input.DataHoraEncerramentoInscricao.HasValue)
            {
                if (input.DataHoraEncerramentoInscricao.Value <= input.DataHoraAberturaInscricao)
                {
                    TempData["MensagemErro"] = "O encerramento das inscrições deve ser posterior à abertura (RN16).";
                    return RedirectToAction(nameof(Index));
                }
                if (ehFinito && input.DataHoraEncerramentoInscricao.Value > inicioPrimeira)
                {
                    TempData["MensagemErro"] = "Em eventos finitos, o encerramento das inscrições não pode ser posterior ao início da primeira ocorrência (RN16).";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Gera a lista de ocorrências previstas (RN01, RN05)
            var ocorrenciasGeradas = GerarOcorrencias(
                inicioPrimeira,
                fimPrimeira,
                classificacao,
                frequencia,
                modalidade,
                qtdTotal,
                dataFinal);

            if (classificacao == "RECORRENTE" && ocorrenciasGeradas.Count < 2)
            {
                TempData["MensagemErro"] = "A combinação de frequência e data final informada gera menos de 2 ocorrências. Ajuste a data final (RN01/RN04).";
                return RedirectToAction(nameof(Index));
            }

            // RN09: Verifica se o responsável possui conflito de horário com outro evento
            int eventoIdIgnorar = input.EventoId ?? 0;
            var ocorrenciasResponsavel = await _context.EventoResponsaveis
                .Where(er => er.ResponsavelId == input.ResponsavelId && er.EventoId != eventoIdIgnorar)
                .SelectMany(er => er.Evento!.Ocorrencias.Where(o => !o.Cancelada))
                .ToListAsync();

            bool conflitoResponsavel = ocorrenciasGeradas.Any(nova =>
                ocorrenciasResponsavel.Any(existente =>
                    nova.Inicio < existente.DataHoraFim && existente.DataHoraInicio < nova.Fim));

            if (conflitoResponsavel)
            {
                TempData["MensagemErro"] = "Conflito de horário: o responsável selecionado já está vinculado a outro evento com ocorrência em horário sobreposto (RN09).";
                return RedirectToAction(nameof(Index));
            }

            if (input.EventoId.HasValue && input.EventoId.Value > 0)
            {
                var ev = await _context.Eventos
                    .Include(e => e.EventoResponsaveis)
                    .Include(e => e.Ocorrencias)
                        .ThenInclude(o => o.Presencas)
                    .FirstOrDefaultAsync(e => e.EventoId == input.EventoId.Value);

                if (ev == null)
                {
                    TempData["MensagemErro"] = "Evento não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                ev.Titulo = input.Titulo.Trim();
                ev.Descricao = input.Descricao.Trim();
                ev.TipoEvento = input.TipoEvento;
                ev.Local = input.Local.Trim();
                ev.Classificacao = classificacao;
                ev.Frequencia = frequencia;
                ev.ModalidadeDuracao = modalidade;
                ev.QuantidadeTotalOcorrencias = qtdTotal;
                ev.DataFinal = dataFinal;
                ev.Capacidade = input.Capacidade;
                ev.DataHoraAberturaInscricao = input.DataHoraAberturaInscricao;
                ev.DataHoraEncerramentoInscricao = input.DataHoraEncerramentoInscricao;
                ev.LimiteFaltasConsecutivas = limiteFaltas;

                _context.EventoResponsaveis.RemoveRange(ev.EventoResponsaveis);
                _context.EventoResponsaveis.Add(new EventoResponsavel
                {
                    EventoId = ev.EventoId,
                    ResponsavelId = input.ResponsavelId
                });

                // Atualiza ocorrências que ainda não possuem presenças registradas
                var ocorrenciasSemPresenca = ev.Ocorrencias.Where(o => !o.Presencas.Any() && !o.ListaPresencaFinalizada).ToList();
                if (ocorrenciasSemPresenca.Count == ev.Ocorrencias.Count)
                {
                    _context.Ocorrencias.RemoveRange(ocorrenciasSemPresenca);
                    foreach (var oc in ocorrenciasGeradas)
                    {
                        _context.Ocorrencias.Add(new Ocorrencia
                        {
                            EventoId = ev.EventoId,
                            DataHoraInicio = oc.Inicio,
                            DataHoraFim = oc.Fim,
                            Cancelada = false,
                            ListaPresencaFinalizada = false
                        });
                    }
                }

                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Evento atualizado com sucesso!";
            }
            else
            {
                // RN41: Todo evento novo inicia OCULTO e PROGRAMADO
                var novoEvento = new Evento
                {
                    Titulo = input.Titulo.Trim(),
                    Descricao = input.Descricao.Trim(),
                    TipoEvento = input.TipoEvento,
                    Local = input.Local.Trim(),
                    Classificacao = classificacao,
                    Frequencia = frequencia,
                    ModalidadeDuracao = modalidade,
                    QuantidadeTotalOcorrencias = qtdTotal,
                    DataFinal = dataFinal,
                    Capacidade = input.Capacidade,
                    DataHoraAberturaInscricao = input.DataHoraAberturaInscricao,
                    DataHoraEncerramentoInscricao = input.DataHoraEncerramentoInscricao,
                    LimiteFaltasConsecutivas = limiteFaltas,
                    EstadoEvento = "PROGRAMADO",
                    Visibilidade = "OCULTO"
                };

                _context.Eventos.Add(novoEvento);
                await _context.SaveChangesAsync();

                _context.EventoResponsaveis.Add(new EventoResponsavel
                {
                    EventoId = novoEvento.EventoId,
                    ResponsavelId = input.ResponsavelId
                });

                foreach (var oc in ocorrenciasGeradas)
                {
                    _context.Ocorrencias.Add(new Ocorrencia
                    {
                        EventoId = novoEvento.EventoId,
                        DataHoraInicio = oc.Inicio,
                        DataHoraFim = oc.Fim,
                        Cancelada = false,
                        ListaPresencaFinalizada = false
                    });
                }

                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = $"Evento '{novoEvento.Titulo}' cadastrado com visibilidade OCULTO ({ocorrenciasGeradas.Count} ocorrência(s) gerada(s)). Clique em 'Publicar' na tabela para exibi-lo na Home (RN41).";
            }

            return RedirectToAction(nameof(Index));
        }

        // RN41-RN42: Alternar visibilidade entre PUBLICADO e OCULTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlternarVisibilidadeEvento(int id)
        {
            var ev = await _context.Eventos.FindAsync(id);
            if (ev == null)
            {
                TempData["MensagemErro"] = "Evento não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (ev.EstadoEvento != "PROGRAMADO")
            {
                TempData["MensagemErro"] = "Somente eventos com estado PROGRAMADO podem ser publicados ou ocultados (RN42).";
                return RedirectToAction(nameof(Index));
            }

            ev.Visibilidade = ev.Visibilidade == "PUBLICADO" ? "OCULTO" : "PUBLICADO";
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = $"Visibilidade do evento '{ev.Titulo}' alterada para {ev.Visibilidade}.";
            return RedirectToAction(nameof(Index));
        }

        // RN43-RN44: Cancelar evento com motivo obrigatório, cancelando ocorrências futuras e inscrições confirmadas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarEvento(int id, string? motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["MensagemErro"] = "O motivo do cancelamento é obrigatório para cancelar um evento (RN43).";
                return RedirectToAction(nameof(Index));
            }

            var ev = await _context.Eventos
                .Include(e => e.Ocorrencias)
                .Include(e => e.Inscricoes)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (ev == null)
            {
                TempData["MensagemErro"] = "Evento não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var agora = DateTime.Now;
            var ocorrenciasFuturas = ev.Ocorrencias.Where(o => !o.Cancelada && o.DataHoraFim > agora).ToList();
            if (!ocorrenciasFuturas.Any() && ev.EstadoEvento != "PROGRAMADO")
            {
                TempData["MensagemErro"] = "Eventos sem ocorrências futuras não podem ser cancelados (RN43).";
                return RedirectToAction(nameof(Index));
            }

            ev.EstadoEvento = "CANCELADO";
            ev.Visibilidade = "OCULTO";
            ev.MotivoCancelamento = motivo.Trim();

            foreach (var oc in ocorrenciasFuturas)
            {
                oc.Cancelada = true;
            }

            foreach (var insc in ev.Inscricoes.Where(i => i.Estado == "CONFIRMADA"))
            {
                insc.Estado = "CANCELADA_POR_EVENTO";
            }

            await _context.SaveChangesAsync();
            TempData["MensagemSucesso"] = $"Evento '{ev.Titulo}' cancelado com sucesso. Ocorrências futuras e inscrições afetadas foram atualizadas (RN43/RN44).";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirEvento(int id)
        {
            var ev = await _context.Eventos
                .Include(e => e.Inscricoes)
                .Include(e => e.Ocorrencias)
                .Include(e => e.EventoResponsaveis)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (ev == null)
            {
                TempData["MensagemErro"] = "Evento não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (ev.Inscricoes.Any())
            {
                TempData["MensagemErro"] = "Este evento já possui inscrições vinculadas. Utilize a opção 'Cancelar' ou 'Ocultar' para preservar o histórico (RN42/RN43).";
                return RedirectToAction(nameof(Index));
            }

            _context.Ocorrencias.RemoveRange(ev.Ocorrencias);
            _context.EventoResponsaveis.RemoveRange(ev.EventoResponsaveis);
            _context.Eventos.Remove(ev);
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = "Evento excluído com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 3. GESTÃO DE INSCRIÇÕES (RN45, RN50)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarInscricao(int id, string? motivo)
        {
            var insc = await _context.Inscricoes
                .Include(i => i.Evento)
                    .ThenInclude(e => e!.Ocorrencias)
                .FirstOrDefaultAsync(i => i.InscricaoId == id);

            if (insc == null)
            {
                TempData["MensagemErro"] = "Inscrição não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            if (insc.Estado != "CONFIRMADA")
            {
                TempData["MensagemErro"] = "Apenas inscrições CONFIRMADAS podem ser canceladas a pedido.";
                return RedirectToAction(nameof(Index));
            }

            insc.Estado = "CANCELADA_A_PEDIDO";
            insc.MotivoCancelamento = string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim();
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = $"Inscrição {insc.CodigoInscricao} alterada para CANCELADA_A_PEDIDO e vaga liberada (RN45).";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReativarInscricao(int id)
        {
            var insc = await _context.Inscricoes
                .Include(i => i.Pessoa)
                .Include(i => i.Evento)
                    .ThenInclude(e => e!.Ocorrencias)
                .Include(i => i.Evento)
                    .ThenInclude(e => e!.Inscricoes)
                        .ThenInclude(ei => ei.Pessoa)
                .FirstOrDefaultAsync(i => i.InscricaoId == id);

            if (insc == null || insc.Evento == null || insc.Pessoa == null)
            {
                TempData["MensagemErro"] = "Inscrição não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            // RN50: Somente CANCELADA_A_PEDIDO ou ENCERRADA_POR_FALTAS podem ser reativadas pela equipe interna
            if (insc.Estado is not ("CANCELADA_A_PEDIDO" or "ENCERRADA_POR_FALTAS"))
            {
                TempData["MensagemErro"] = $"Inscrições no estado '{insc.Estado}' não podem ser reativadas por esta ação (RN50).";
                return RedirectToAction(nameof(Index));
            }

            if (insc.Pessoa.Situacao != "ATIVA")
            {
                TempData["MensagemErro"] = "Não é possível reativar a inscrição porque o participante está com cadastro INATIVO.";
                return RedirectToAction(nameof(Index));
            }

            var agora = DateTime.Now;
            var ocorrenciasFuturas = insc.Evento.Ocorrencias
                .Where(o => !o.Cancelada && o.DataHoraFim > agora)
                .ToList();

            if (insc.Evento.EstadoEvento != "PROGRAMADO" || !ocorrenciasFuturas.Any())
            {
                TempData["MensagemErro"] = "Não é possível reativar a inscrição porque o evento não possui ocorrências futuras ativas (RN50).";
                return RedirectToAction(nameof(Index));
            }

            var ocupadas = insc.Evento.Inscricoes.Count(i =>
                i.Estado == "CONFIRMADA" &&
                (i.Pessoa == null || i.Pessoa.Situacao == "ATIVA"));

            if (ocupadas >= (int)insc.Evento.Capacidade)
            {
                TempData["MensagemErro"] = "Não há vagas disponíveis no evento para reativar esta inscrição (RN50).";
                return RedirectToAction(nameof(Index));
            }

            // Verifica conflito de horário (RN50)
            var outrasOcorrencias = await _context.Inscricoes
                .Where(i => i.PessoaId == insc.PessoaId &&
                            i.EventoId != insc.EventoId &&
                            i.Estado == "CONFIRMADA")
                .SelectMany(i => i.Evento!.Ocorrencias.Where(o => !o.Cancelada && o.DataHoraFim > agora))
                .ToListAsync();

            bool temConflito = ocorrenciasFuturas.Any(o1 =>
                outrasOcorrencias.Any(o2 =>
                    o1.DataHoraInicio < o2.DataHoraFim && o2.DataHoraInicio < o1.DataHoraFim));

            if (temConflito)
            {
                TempData["MensagemErro"] = "Não foi possível reativar: o participante possui outra inscrição confirmada em horário sobreposto (RN50).";
                return RedirectToAction(nameof(Index));
            }

            insc.Estado = "CONFIRMADA";
            insc.FaltasConsecutivas = 0;
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = $"Inscrição {insc.CodigoInscricao} reativada para CONFIRMADA com sucesso (RN50)!";
            return RedirectToAction(nameof(Index));
        }

        private static List<(DateTime Inicio, DateTime Fim)> GerarOcorrencias(
            DateTime inicioPrimeira,
            DateTime fimPrimeira,
            string classificacao,
            string? frequencia,
            string? modalidade,
            uint? qtdTotal,
            DateTime? dataFinal)
        {
            var lista = new List<(DateTime Inicio, DateTime Fim)>();
            if (classificacao == "UNICO")
            {
                lista.Add((inicioPrimeira, fimPrimeira));
                return lista;
            }

            int maxOcorrencias = modalidade switch
            {
                "QUANTIDADE_OCORRENCIAS" => (int)Math.Min(qtdTotal ?? 2, 52),
                "SEM_PREVISAO_DE_FIM" => 4, // Gera progressivamente as primeiras 4 ocorrências da série contínua (RN05)
                _ => 52
            };

            var duracao = fimPrimeira - inicioPrimeira;
            for (int i = 0; i < maxOcorrencias; i++)
            {
                DateTime inicioAtual = frequencia switch
                {
                    "DIARIA" => inicioPrimeira.AddDays(i),
                    "SEMANAL" => inicioPrimeira.AddDays(i * 7),
                    "QUINZENAL" => inicioPrimeira.AddDays(i * 14),
                    "MENSAL" => inicioPrimeira.AddMonths(i),
                    _ => inicioPrimeira.AddDays(i * 7)
                };

                if (modalidade == "DATA_FINAL" && dataFinal.HasValue && inicioAtual.Date > dataFinal.Value.Date)
                    break;

                lista.Add((inicioAtual, inicioAtual.Add(duracao)));
            }

            return lista;
        }

        private async Task<AdminPainelViewModel> MontarPainelViewModelAsync()
        {
            var vm = new AdminPainelViewModel
            {
                UsuarioLogado = User.FindFirst("NomePessoa")?.Value ?? User.Identity?.Name ?? "Admin",
                PapelLogado = User.FindFirst(ClaimTypes.Role)?.Value ?? "ADMINISTRADOR"
            };

            try
            {
                vm.Responsaveis = await _context.Responsaveis
                    .Include(r => r.Pessoa)
                        .ThenInclude(p => p!.Endereco)
                    .Include(r => r.EventoResponsaveis)
                    .OrderBy(r => r.Pessoa!.Nome)
                    .ToListAsync();

                vm.Eventos = await _context.Eventos
                    .Include(e => e.EventoResponsaveis)
                        .ThenInclude(er => er.Responsavel)
                            .ThenInclude(r => r!.Pessoa)
                    .Include(e => e.Ocorrencias)
                    .Include(e => e.Inscricoes)
                        .ThenInclude(i => i.Pessoa)
                    .OrderByDescending(e => e.EventoId)
                    .ToListAsync();

                vm.Inscricoes = await _context.Inscricoes
                    .Include(i => i.Pessoa)
                    .Include(i => i.Evento)
                    .OrderByDescending(i => i.InscricaoId)
                    .ToListAsync();
            }
            catch
            {
                vm.MensagemErro = "Não foi possível carregar os dados do MySQL. Verifique se o serviço MySQL (localhost:3306) está rodando.";
            }

            return vm;
        }
    }
}
