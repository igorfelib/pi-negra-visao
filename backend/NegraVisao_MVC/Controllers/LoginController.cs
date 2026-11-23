using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NegraVisao_MVC.Data;

namespace NegraVisao_MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entrar(string? usuario, string? senha)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro = "* campo obrigatorio";
                return View("Index");
            }

            try
            {
                var loginLimpo = usuario.Trim();
                var senhaLimpa = senha.Trim();

                var conta = await _context.UsuariosInternos
                    .Include(u => u.Pessoa)
                        .ThenInclude(p => p!.PessoaPapeis)
                            .ThenInclude(pp => pp.Papel)
                    .FirstOrDefaultAsync(u => u.Login == loginLimpo && u.Senha == senhaLimpa);

                if (conta == null)
                {
                    ViewBag.Erro = "usuario e senha não correspondem";
                    return View("Index");
                }

                // RN35: Situação de acesso interno deve estar ATIVO
                if (conta.SituacaoAcesso != "ATIVO")
                {
                    ViewBag.Erro = "Este acesso interno encontra-se INATIVO (RN35).";
                    return View("Index");
                }

                var papeis = conta.Pessoa?.PessoaPapeis
                    .Where(pp => pp.Papel != null)
                    .Select(pp => pp.Papel!.NomePapel)
                    .ToList() ?? new List<string>();

                // RN35-RN38: Deve possuir perfil COLABORADOR ou ADMINISTRADOR
                string? papelInterno = papeis.Contains("ADMINISTRADOR")
                    ? "ADMINISTRADOR"
                    : (papeis.Contains("COLABORADOR") ? "COLABORADOR" : null);

                if (papelInterno == null)
                {
                    ViewBag.Erro = "Usuário não possui perfil interno de Colaborador ou Administrador (RN35).";
                    return View("Index");
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, conta.UsuarioInternoId.ToString()),
                    new(ClaimTypes.Name, conta.Login),
                    new("NomePessoa", conta.Pessoa?.Nome ?? conta.Login),
                    new(ClaimTypes.Role, papelInterno)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception)
            {
                ViewBag.Erro = "Erro ao conectar ao banco MySQL (localhost:3306). Verifique se o servidor MySQL está ativo.";
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
