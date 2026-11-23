using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NegraVisao_MVC.Data;
using NegraVisao_MVC.Helpers;
using NegraVisao_MVC.Models;

namespace NegraVisao_MVC.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly AppDbContext _context;

        public InscricaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? eventoId)
        {
            var vm = new InscricaoFormViewModel
            {
                EventoId = eventoId,
                EventosDisponiveis = await CarregarEventosDisponiveisAsync()
            };

            if (TempData["CodigoConfirmado"] is string codigo)
            {
                vm.CodigoConfirmado = codigo;
                vm.EventoConfirmadoTitulo = TempData["EventoConfirmadoTitulo"] as string;
                vm.EventoConfirmadoMeta = TempData["EventoConfirmadoMeta"] as string;
            }

            return View(vm);
        }

        // RN26-RN27: Localiza cadastro pelo CPF no fluxo público e retorna apenas dados mascarados
        [HttpGet]
        public async Task<IActionResult> BuscarPorCpf(string? cpf)
        {
            var cpfLimpo = ValidacaoHelper.SomenteDigitos(cpf);
            if (!ValidacaoHelper.ValidarCpf(cpfLimpo))
            {
                return Json(new { encontrado = false, cpfInvalido = true, mensagem = "CPF inválido. Informe 11 dígitos válidos." });
            }

            var pessoa = await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Cpf == cpfLimpo);

            if (pessoa == null)
            {
                return Json(new { encontrado = false, cpfInvalido = false });
            }

            if (pessoa.Situacao == "INATIVA")
            {
                return Json(new
                {
                    encontrado = true,
                    inativo = true,
                    mensagem = "E03 - Este cadastro encontra-se inativo. A inscrição não pode prosseguir no fluxo público (RN21)."
                });
            }

            return Json(new
            {
                encontrado = true,
                inativo = false,
                dados = new
                {
                    nome = ValidacaoHelper.MascararTexto(pessoa.Nome),
                    dataNascimento = ValidacaoHelper.MascararData(pessoa.DataNascimento),
                    telefone = ValidacaoHelper.MascararTelefone(pessoa.Telefone),
                    email = ValidacaoHelper.MascararEmail(pessoa.Email),
                    cep = ValidacaoHelper.MascararCep(pessoa.Endereco?.Cep),
                    logradouro = ValidacaoHelper.MascararTexto(pessoa.Endereco?.Logradouro),
                    numeroImovel = ValidacaoHelper.MascararNumeroImovel(pessoa.Endereco?.NumeroImovel),
                    complemento = ValidacaoHelper.MascararTexto(pessoa.Endereco?.Complemento),
                    bairro = ValidacaoHelper.MascararTexto(pessoa.Endereco?.Bairro),
                    municipio = ValidacaoHelper.MascararTexto(pessoa.Endereco?.Municipio),
                    uf = ValidacaoHelper.MascararUf(pessoa.Endereco?.Uf)
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salvar(InscricaoFormViewModel form)
        {
            form.EventosDisponiveis = await CarregarEventosDisponiveisAsync();

            if (!form.EventoId.HasValue || form.EventoId.Value <= 0)
            {
                form.MensagemErro = "Selecione um evento válido para realizar a inscrição.";
                return View("Index", form);
            }

            var cpfLimpo = ValidacaoHelper.SomenteDigitos(form.Cpf);
            if (!ValidacaoHelper.ValidarCpf(cpfLimpo))
            {
                form.MensagemErro = "E01 - CPF inválido. O CPF deve possuir 11 dígitos e verificadores válidos (RN24).";
                return View("Index", form);
            }

            var agora = DateTime.Now;
            var evento = await _context.Eventos
                .Include(e => e.Ocorrencias)
                .Include(e => e.Inscricoes)
                    .ThenInclude(i => i.Pessoa)
                .FirstOrDefaultAsync(e => e.EventoId == form.EventoId.Value);

            // E06 - Validação de disponibilidade e janela de inscrição (RN16-RN18, RN42)
            if (evento == null ||
                evento.EstadoEvento != "PROGRAMADO" ||
                evento.Visibilidade != "PUBLICADO" ||
                evento.DataHoraAberturaInscricao > agora ||
                (evento.DataHoraEncerramentoInscricao.HasValue && evento.DataHoraEncerramentoInscricao.Value < agora))
            {
                form.MensagemErro = "E06 - Evento indisponível ou fora do período de inscrições abertas (RN17).";
                return View("Index", form);
            }

            var pessoaExistente = await _context.Pessoas
                .Include(p => p.Endereco)
                .Include(p => p.PessoaPapeis)
                .FirstOrDefaultAsync(p => p.Cpf == cpfLimpo);

            // E03 - Pessoa inativa não pode se inscrever nem ser reativada no fluxo público (RN21)
            if (pessoaExistente != null && pessoaExistente.Situacao == "INATIVA")
            {
                form.MensagemErro = "E03 - O cadastro associado a este CPF está inativo. Regularize junto à administração da associação (RN21).";
                return View("Index", form);
            }

            // RN28: Se a pessoa já existe, preserva campos mascarados que não foram alterados e valida substituições completas
            string nomeFinal;
            DateTime dataNascFinal;
            string? emailFinal;
            string? telefoneFinal;
            string cepFinal;
            string logradouroFinal;
            string numeroFinal;
            string? complementoFinal;
            string bairroFinal;
            string municipioFinal;
            string ufFinal;

            if (pessoaExistente != null)
            {
                nomeFinal = ValidacaoHelper.ContemMascara(form.Nome) ? pessoaExistente.Nome : (form.Nome ?? string.Empty).Trim();

                if (ValidacaoHelper.ContemMascara(form.DataNascimento))
                {
                    dataNascFinal = pessoaExistente.DataNascimento;
                }
                else if (!TentarConverterData(form.DataNascimento, out dataNascFinal))
                {
                    form.MensagemErro = "E02 - Informe uma data de nascimento válida (RN29).";
                    return View("Index", form);
                }

                emailFinal = ValidacaoHelper.ContemMascara(form.Email)
                    ? pessoaExistente.Email
                    : (string.IsNullOrWhiteSpace(form.Email) ? null : form.Email.Trim());

                telefoneFinal = ValidacaoHelper.ContemMascara(form.Telefone)
                    ? pessoaExistente.Telefone
                    : (string.IsNullOrWhiteSpace(form.Telefone) ? null : ValidacaoHelper.SomenteDigitos(form.Telefone));

                var end = pessoaExistente.Endereco;
                cepFinal = (ValidacaoHelper.ContemMascara(form.Cep) && end != null)
                    ? end.Cep
                    : ValidacaoHelper.SomenteDigitos(form.Cep);

                logradouroFinal = (ValidacaoHelper.ContemMascara(form.Logradouro) && end != null)
                    ? end.Logradouro
                    : (form.Logradouro ?? string.Empty).Trim();

                numeroFinal = (ValidacaoHelper.ContemMascara(form.NumeroImovel) && end != null)
                    ? end.NumeroImovel
                    : (form.NumeroImovel ?? string.Empty).Trim();

                complementoFinal = (ValidacaoHelper.ContemMascara(form.Complemento) && end != null)
                    ? end.Complemento
                    : (string.IsNullOrWhiteSpace(form.Complemento) ? null : form.Complemento.Trim());

                bairroFinal = (ValidacaoHelper.ContemMascara(form.Bairro) && end != null)
                    ? end.Bairro
                    : (form.Bairro ?? string.Empty).Trim();

                municipioFinal = (ValidacaoHelper.ContemMascara(form.Municipio) && end != null)
                    ? end.Municipio
                    : (form.Municipio ?? string.Empty).Trim();

                ufFinal = (ValidacaoHelper.ContemMascara(form.Uf) && end != null)
                    ? end.Uf
                    : (form.Uf ?? string.Empty).Trim().ToUpperInvariant();
            }
            else
            {
                nomeFinal = (form.Nome ?? string.Empty).Trim();
                if (!TentarConverterData(form.DataNascimento, out dataNascFinal))
                {
                    form.MensagemErro = "E02 - Informe uma data de nascimento válida (RN23/RN29).";
                    return View("Index", form);
                }
                emailFinal = string.IsNullOrWhiteSpace(form.Email) ? null : form.Email.Trim();
                telefoneFinal = string.IsNullOrWhiteSpace(form.Telefone) ? null : ValidacaoHelper.SomenteDigitos(form.Telefone);
                cepFinal = ValidacaoHelper.SomenteDigitos(form.Cep);
                logradouroFinal = (form.Logradouro ?? string.Empty).Trim();
                numeroFinal = (form.NumeroImovel ?? string.Empty).Trim();
                complementoFinal = string.IsNullOrWhiteSpace(form.Complemento) ? null : form.Complemento.Trim();
                bairroFinal = (form.Bairro ?? string.Empty).Trim();
                municipioFinal = (form.Municipio ?? string.Empty).Trim();
                ufFinal = (form.Uf ?? string.Empty).Trim().ToUpperInvariant();
            }

            // Validações de dados pessoais e endereço (RN23, RN28-RN34)
            if (string.IsNullOrWhiteSpace(nomeFinal) || ValidacaoHelper.ContemMascara(nomeFinal))
            {
                form.MensagemErro = "E02 - O nome completo é obrigatório e não pode conter máscaras parciais (RN28/RN29).";
                return View("Index", form);
            }

            if (dataNascFinal.Date > DateTime.Today)
            {
                form.MensagemErro = "E02 - A data de nascimento não pode estar no futuro (RN29).";
                return View("Index", form);
            }

            if (!string.IsNullOrEmpty(emailFinal) &&
                (ValidacaoHelper.ContemMascara(emailFinal) || !Regex.IsMatch(emailFinal, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
            {
                form.MensagemErro = "E02 - O e-mail informado possui formato inválido (RN33).";
                return View("Index", form);
            }

            if (!string.IsNullOrEmpty(telefoneFinal) && (telefoneFinal.Length < 10 || telefoneFinal.Length > 11))
            {
                form.MensagemErro = "E02 - O telefone deve conter DDD e 10 ou 11 dígitos numéricos (RN34).";
                return View("Index", form);
            }

            if (cepFinal.Length != 8)
            {
                form.MensagemErro = "E02 - O CEP é obrigatório e deve conter 8 dígitos numéricos (RN30).";
                return View("Index", form);
            }

            if (string.IsNullOrWhiteSpace(logradouroFinal) ||
                string.IsNullOrWhiteSpace(numeroFinal) ||
                string.IsNullOrWhiteSpace(bairroFinal) ||
                string.IsNullOrWhiteSpace(municipioFinal) ||
                ufFinal.Length != 2 ||
                ValidacaoHelper.ContemMascara(logradouroFinal) ||
                ValidacaoHelper.ContemMascara(numeroFinal) ||
                ValidacaoHelper.ContemMascara(bairroFinal) ||
                ValidacaoHelper.ContemMascara(municipioFinal) ||
                ValidacaoHelper.ContemMascara(ufFinal))
            {
                form.MensagemErro = "E02 - Preencha todos os campos obrigatórios de endereço (Logradouro, Número ou S/N, Bairro, Município e UF com 2 letras) sem máscaras (RN32).";
                return View("Index", form);
            }

            // Verifica inscrição existente para Pessoa + Evento (RN11, RN12, E04, E05, A06)
            Inscricao? inscricaoExistente = null;
            if (pessoaExistente != null)
            {
                inscricaoExistente = await _context.Inscricoes
                    .FirstOrDefaultAsync(i => i.PessoaId == pessoaExistente.PessoaId && i.EventoId == evento.EventoId);

                if (inscricaoExistente != null)
                {
                    if (inscricaoExistente.Estado == "CONFIRMADA")
                    {
                        form.MensagemErro = $"E04 - Você já possui inscrição CONFIRMADA neste evento (Código: {inscricaoExistente.CodigoInscricao}).";
                        return View("Index", form);
                    }

                    if (inscricaoExistente.Estado is "CANCELADA_A_PEDIDO" or "ENCERRADA_POR_FALTAS" or "CANCELADA_POR_EVENTO")
                    {
                        form.MensagemErro = $"E05 - Sua inscrição neste evento está com status '{inscricaoExistente.Estado}'. A reativação depende da equipe interna da associação (RN50).";
                        return View("Index", form);
                    }
                }
            }

            // E07 - Verifica disponibilidade de vagas (RN14-RN15)
            var ocupadas = evento.Inscricoes.Count(i =>
                i.Estado == "CONFIRMADA" &&
                (i.Pessoa == null || i.Pessoa.Situacao == "ATIVA"));

            if (ocupadas >= (int)evento.Capacidade)
            {
                form.MensagemErro = "E07 - Evento lotado. Todas as vagas disponíveis já foram preenchidas (RN14).";
                return View("Index", form);
            }

            // E08 - Verifica conflito de horário com outras inscrições CONFIRMADAS da pessoa (RN08, RN10)
            if (pessoaExistente != null)
            {
                var ocorrenciasNovoEvento = evento.Ocorrencias
                    .Where(o => !o.Cancelada && o.DataHoraFim > agora)
                    .ToList();

                var outrasOcorrenciasConfirmadas = await _context.Inscricoes
                    .Where(i => i.PessoaId == pessoaExistente.PessoaId &&
                                i.EventoId != evento.EventoId &&
                                i.Estado == "CONFIRMADA")
                    .SelectMany(i => i.Evento!.Ocorrencias.Where(o => !o.Cancelada && o.DataHoraFim > agora))
                    .ToListAsync();

                bool temConflito = ocorrenciasNovoEvento.Any(o1 =>
                    outrasOcorrenciasConfirmadas.Any(o2 =>
                        o1.DataHoraInicio < o2.DataHoraFim && o2.DataHoraInicio < o1.DataHoraFim));

                if (temConflito)
                {
                    form.MensagemErro = "E08 - Conflito de horário: você já possui inscrição confirmada em outro evento com ocorrência em horário sobreposto (RN10).";
                    return View("Index", form);
                }
            }

            // Transação para salvar Pessoa + Endereco + Papel PARTICIPANTE + Inscricao (FP08-FP10)
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (pessoaExistente == null)
                {
                    pessoaExistente = new Pessoa
                    {
                        Cpf = cpfLimpo,
                        Nome = nomeFinal,
                        DataNascimento = dataNascFinal.Date,
                        Email = emailFinal,
                        Telefone = telefoneFinal,
                        Situacao = "ATIVA"
                    };
                    _context.Pessoas.Add(pessoaExistente);
                    await _context.SaveChangesAsync();

                    _context.Enderecos.Add(new Endereco
                    {
                        PessoaId = pessoaExistente.PessoaId,
                        Cep = cepFinal,
                        Logradouro = logradouroFinal,
                        NumeroImovel = numeroFinal,
                        Complemento = complementoFinal,
                        Bairro = bairroFinal,
                        Municipio = municipioFinal,
                        Uf = ufFinal
                    });
                }
                else
                {
                    pessoaExistente.Nome = nomeFinal;
                    pessoaExistente.DataNascimento = dataNascFinal.Date;
                    pessoaExistente.Email = emailFinal;
                    pessoaExistente.Telefone = telefoneFinal;

                    if (pessoaExistente.Endereco == null)
                    {
                        _context.Enderecos.Add(new Endereco
                        {
                            PessoaId = pessoaExistente.PessoaId,
                            Cep = cepFinal,
                            Logradouro = logradouroFinal,
                            NumeroImovel = numeroFinal,
                            Complemento = complementoFinal,
                            Bairro = bairroFinal,
                            Municipio = municipioFinal,
                            Uf = ufFinal
                        });
                    }
                    else
                    {
                        pessoaExistente.Endereco.Cep = cepFinal;
                        pessoaExistente.Endereco.Logradouro = logradouroFinal;
                        pessoaExistente.Endereco.NumeroImovel = numeroFinal;
                        pessoaExistente.Endereco.Complemento = complementoFinal;
                        pessoaExistente.Endereco.Bairro = bairroFinal;
                        pessoaExistente.Endereco.Municipio = municipioFinal;
                        pessoaExistente.Endereco.Uf = ufFinal;
                    }
                }

                // Garante papel PARTICIPANTE (RN40)
                var papelParticipante = await _context.Papeis.FirstOrDefaultAsync(p => p.NomePapel == "PARTICIPANTE");
                if (papelParticipante == null)
                {
                    papelParticipante = new Papel { NomePapel = "PARTICIPANTE" };
                    _context.Papeis.Add(papelParticipante);
                    await _context.SaveChangesAsync();
                }

                bool jaTemPapel = await _context.PessoaPapeis.AnyAsync(pp =>
                    pp.PessoaId == pessoaExistente.PessoaId && pp.PapelId == papelParticipante.PapelId);

                if (!jaTemPapel)
                {
                    _context.PessoaPapeis.Add(new PessoaPapel
                    {
                        PessoaId = pessoaExistente.PessoaId,
                        PapelId = papelParticipante.PapelId
                    });
                }

                string codigoInscricao;
                if (inscricaoExistente != null)
                {
                    // A06: Reutiliza registro INVALIDADA_POR_INATIVACAO quando pessoa já está ativa
                    inscricaoExistente.Estado = "CONFIRMADA";
                    inscricaoExistente.FaltasConsecutivas = 0;
                    if (string.IsNullOrWhiteSpace(inscricaoExistente.CodigoInscricao))
                        inscricaoExistente.CodigoInscricao = await GerarCodigoUnicoInscricaoAsync();
                    codigoInscricao = inscricaoExistente.CodigoInscricao;
                }
                else
                {
                    codigoInscricao = await GerarCodigoUnicoInscricaoAsync();
                    _context.Inscricoes.Add(new Inscricao
                    {
                        PessoaId = pessoaExistente.PessoaId,
                        EventoId = evento.EventoId,
                        CodigoInscricao = codigoInscricao,
                        Estado = "CONFIRMADA",
                        FaltasConsecutivas = 0
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var proxOcorrencia = evento.Ocorrencias
                    .Where(o => !o.Cancelada)
                    .OrderBy(o => o.DataHoraInicio)
                    .FirstOrDefault();

                var metaConfirmacao = proxOcorrencia != null
                    ? $"{proxOcorrencia.DataHoraInicio:dd/MM/yyyy 'às' HH:mm} · {evento.Local}"
                    : evento.Local;

                TempData["CodigoConfirmado"] = codigoInscricao;
                TempData["EventoConfirmadoTitulo"] = evento.Titulo;
                TempData["EventoConfirmadoMeta"] = metaConfirmacao;

                return RedirectToAction(nameof(Index), new { eventoId = evento.EventoId });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                form.MensagemErro = "E09 - Falha técnica ao confirmar a inscrição. Nenhum dado parcial foi salvo.";
                return View("Index", form);
            }
        }

        private async Task<string> GerarCodigoUnicoInscricaoAsync()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            while (true)
            {
                var buffer = new char[6];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
                }
                var codigo = $"NV-{new string(buffer)}";
                if (!await _context.Inscricoes.AnyAsync(i => i.CodigoInscricao == codigo))
                    return codigo;
            }
        }

        private static bool TentarConverterData(string? texto, out DateTime data)
        {
            data = default;
            if (string.IsNullOrWhiteSpace(texto)) return false;

            var formatos = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy" };
            return DateTime.TryParseExact(
                texto.Trim(),
                formatos,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out data);
        }

        private async Task<List<EventoPublicoViewModel>> CarregarEventosDisponiveisAsync()
        {
            var resultado = new List<EventoPublicoViewModel>();
            try
            {
                var agora = DateTime.Now;
                var eventos = await _context.Eventos
                    .Include(e => e.Ocorrencias)
                    .Include(e => e.Inscricoes)
                        .ThenInclude(i => i.Pessoa)
                    .Where(e => e.Visibilidade == "PUBLICADO" && e.EstadoEvento == "PROGRAMADO")
                    .OrderBy(e => e.Titulo)
                    .ToListAsync();

                foreach (var ev in eventos)
                {
                    var confirmadas = ev.Inscricoes.Count(i =>
                        i.Estado == "CONFIRMADA" &&
                        (i.Pessoa == null || i.Pessoa.Situacao == "ATIVA"));

                    var vagas = Math.Max(0, (int)ev.Capacidade - confirmadas);
                    var dentroJanela = ev.DataHoraAberturaInscricao <= agora &&
                        (!ev.DataHoraEncerramentoInscricao.HasValue || ev.DataHoraEncerramentoInscricao.Value >= agora);

                    var prox = ev.Ocorrencias
                        .Where(o => !o.Cancelada)
                        .OrderBy(o => o.DataHoraInicio)
                        .FirstOrDefault();

                    var dataHora = prox != null
                        ? $"{prox.DataHoraInicio:dd/MM/yyyy HH:mm}"
                        : "Horário a definir";

                    resultado.Add(new EventoPublicoViewModel
                    {
                        EventoId = ev.EventoId,
                        Titulo = ev.Titulo,
                        TipoEvento = ev.TipoEvento,
                        Local = ev.Local,
                        DataExibicao = dataHora,
                        VagasDisponiveis = vagas,
                        Capacidade = ev.Capacidade,
                        InscricoesAbertas = dentroJanela && vagas > 0
                    });
                }
            }
            catch
            {
                // Retorna lista vazia caso o MySQL esteja offline
            }
            return resultado;
        }
    }
}
