using System.Globalization;
using System.Text.RegularExpressions;

namespace NegraVisao_MVC.Helpers
{
    public static class ValidacaoHelper
    {
        public static string SomenteDigitos(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return Regex.Replace(valor, @"\D", "");
        }

        public static bool ValidarCpf(string? cpf)
        {
            var limpo = SomenteDigitos(cpf);
            if (limpo.Length != 11)
                return false;

            if (new string(limpo[0], 11) == limpo)
                return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (limpo[i] - '0') * (10 - i);

            int resto = (soma * 10) % 11;
            if (resto == 10) resto = 0;
            if (resto != (limpo[9] - '0'))
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (limpo[i] - '0') * (11 - i);

            resto = (soma * 10) % 11;
            if (resto == 10) resto = 0;

            return resto == (limpo[10] - '0');
        }

        public static string FormatarCpf(string? cpf)
        {
            var d = SomenteDigitos(cpf);
            if (d.Length != 11) return cpf ?? string.Empty;
            return $"{d[..3]}.{d.Substring(3, 3)}.{d.Substring(6, 3)}-{d.Substring(9, 2)}";
        }

        public static string FormatarCep(string? cep)
        {
            var d = SomenteDigitos(cep);
            if (d.Length != 8) return cep ?? string.Empty;
            return $"{d[..5]}-{d.Substring(5, 3)}";
        }

        public static string FormatarTelefone(string? tel)
        {
            var d = SomenteDigitos(tel);
            if (d.Length == 11)
                return $"({d[..2]}) {d.Substring(2, 5)}-{d.Substring(7, 4)}";
            if (d.Length == 10)
                return $"({d[..2]}) {d.Substring(2, 4)}-{d.Substring(6, 4)}";
            return tel ?? string.Empty;
        }

        // RN27: Mascaramento de dados para retorno no fluxo público
        public static bool ContemMascara(string? valor)
        {
            return !string.IsNullOrEmpty(valor) && (valor.Contains('*') || valor.Contains('•'));
        }

        public static string MascararTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var palavras = texto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var mascaradas = palavras.Select(p =>
            {
                if (p.Length <= 1) return "*";
                return p[0] + new string('*', p.Length - 1);
            });
            return string.Join(" ", mascaradas);
        }

        public static string MascararData(DateTime data)
        {
            return $"**/**/{data:yyyy}";
        }

        public static string MascararCep(string? cep)
        {
            var d = SomenteDigitos(cep);
            if (d.Length != 8) return "*****-***";
            return $"{d[..2]}***-***";
        }

        public static string MascararNumeroImovel(string? numero)
        {
            if (string.IsNullOrWhiteSpace(numero)) return "";
            if (numero.Trim().Equals("S/N", StringComparison.OrdinalIgnoreCase)) return "S/N";
            if (numero.Length == 1) return "*";
            return new string('*', numero.Length - 1) + numero[^1];
        }

        public static string MascararUf(string? uf)
        {
            if (string.IsNullOrWhiteSpace(uf)) return "";
            return uf[0] + "*";
        }

        public static string MascararEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return "";
            var partes = email.Split('@');
            if (partes.Length != 2) return MascararTexto(email);
            var usuario = partes[0].Length > 1 ? partes[0][0] + new string('*', Math.Max(3, partes[0].Length - 1)) : "*";
            var dominioPartes = partes[1].Split('.');
            var dom = dominioPartes[0].Length > 1 ? dominioPartes[0][0] + new string('*', Math.Max(3, dominioPartes[0].Length - 1)) : "*";
            var sufixo = dominioPartes.Length > 1 ? "." + string.Join(".", dominioPartes.Skip(1)) : "";
            return $"{usuario}@{dom}{sufixo}";
        }

        public static string MascararTelefone(string? telefone)
        {
            var d = SomenteDigitos(telefone);
            if (string.IsNullOrEmpty(d)) return "";
            if (d.Length >= 10)
                return $"({d[..2]}) *****-**{d[^2..]}";
            return new string('*', Math.Max(1, d.Length - 2)) + (d.Length >= 2 ? d[^2..] : "");
        }

        public static string ResolverImagemEvento(string? titulo, string? tipoEvento)
        {
            var t = (titulo ?? string.Empty).ToLowerInvariant();
            if (t.Contains("capoeira")) return "/img/capoeira.jpeg";
            if (t.Contains("colo") || t.Contains("mãe") || t.Contains("mae")) return "/img/encontro_de_mae.jpg";
            if (t.Contains("cine")) return "/img/cine_visao.jpeg";
            if (t.Contains("dança") || t.Contains("danca") || t.Contains("samba")) return "/img/danca.jpg";
            if (t.Contains("teatro") || t.Contains("ana julia") || t.Contains("racismo")) return "/img/racismo_nosso_de_cada_dia.jpg";
            if (t.Contains("quinta")) return "/img/quinta_coletiva.jpeg";
            if (t.Contains("yoga") || t.Contains("aya")) return "/img/yoga.jpg";

            return (tipoEvento ?? string.Empty).ToUpperInvariant() switch
            {
                "CURSO" => "/img/capoeira.jpeg",
                "PALESTRA" => "/img/quinta_coletiva.jpeg",
                _ => "/img/danca.jpg"
            };
        }

        public static string FormatarDataCurtaPtBr(DateTime data)
        {
            var cultura = new CultureInfo("pt-BR");
            var mes = cultura.DateTimeFormat.GetMonthName(data.Month);
            if (!string.IsNullOrEmpty(mes))
                mes = char.ToUpper(mes[0]) + mes[1..];
            return $"{data:dd} {mes}";
        }
    }
}
