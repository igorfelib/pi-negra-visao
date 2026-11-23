using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NegraVisao_MVC.Data;
using NegraVisao_MVC.Helpers;
using NegraVisao_MVC.Models;

namespace NegraVisao_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var listaEventos = new List<EventoPublicoViewModel>();

            try
            {
                var agora = DateTime.Now;
                var eventos = await _context.Eventos
                    .Include(e => e.Ocorrencias)
                    .Include(e => e.Inscricoes)
                        .ThenInclude(i => i.Pessoa)
                    .Where(e => e.Visibilidade == "PUBLICADO" && e.EstadoEvento == "PROGRAMADO")
                    .OrderBy(e => e.EventoId)
                    .ToListAsync();

                foreach (var ev in eventos)
                {
                    var proximaOcorrencia = ev.Ocorrencias
                        .Where(o => !o.Cancelada)
                        .OrderBy(o => o.DataHoraInicio)
                        .FirstOrDefault();

                    var confirmadasAtivas = ev.Inscricoes.Count(i =>
                        i.Estado == "CONFIRMADA" &&
                        (i.Pessoa == null || i.Pessoa.Situacao == "ATIVA"));

                    var vagasDisponiveis = Math.Max(0, (int)ev.Capacidade - confirmadasAtivas);

                    var dentroJanela = ev.DataHoraAberturaInscricao <= agora &&
                        (!ev.DataHoraEncerramentoInscricao.HasValue || ev.DataHoraEncerramentoInscricao.Value >= agora);

                    var dataTexto = proximaOcorrencia != null
                        ? ValidacaoHelper.FormatarDataCurtaPtBr(proximaOcorrencia.DataHoraInicio)
                        : "A definir";

                    var horaTexto = proximaOcorrencia != null
                        ? proximaOcorrencia.DataHoraInicio.ToString("HH:mm")
                        : "--:--";

                    var statusTexto = !dentroJanela
                        ? "Inscrições encerradas"
                        : (vagasDisponiveis <= 0 ? "Lotado" : $"{vagasDisponiveis} vagas disponíveis");

                    listaEventos.Add(new EventoPublicoViewModel
                    {
                        EventoId = ev.EventoId,
                        Titulo = ev.Titulo,
                        Descricao = ev.Descricao,
                        TipoEvento = ev.TipoEvento,
                        Local = ev.Local,
                        Classificacao = ev.Classificacao,
                        Frequencia = ev.Frequencia,
                        DataExibicao = dataTexto,
                        HorarioExibicao = horaTexto,
                        MetaModal = $"{ev.TipoEvento} · {dataTexto} · {horaTexto} · {ev.Local} ({statusTexto})",
                        ImagemUrl = ValidacaoHelper.ResolverImagemEvento(ev.Titulo, ev.TipoEvento),
                        Capacidade = ev.Capacidade,
                        VagasDisponiveis = vagasDisponiveis,
                        InscricoesAbertas = dentroJanela && vagasDisponiveis > 0,
                        StatusInscricaoTexto = statusTexto
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Não foi possível carregar eventos do MySQL na Home.");
                ViewBag.AvisoBanco = "Aviso: Banco de dados MySQL não conectado no momento. Inicie o serviço MySQL (localhost:3306) para carregar os dados reais.";
            }

            return View(listaEventos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
