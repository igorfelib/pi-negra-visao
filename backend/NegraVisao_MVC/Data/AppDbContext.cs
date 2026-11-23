using Microsoft.EntityFrameworkCore;
using NegraVisao_MVC.Models;

namespace NegraVisao_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Papel> Papeis { get; set; }
        public DbSet<PessoaPapel> PessoaPapeis { get; set; }
        public DbSet<UsuarioInterno> UsuariosInternos { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<EventoResponsavel> EventoResponsaveis { get; set; }
        public DbSet<Ocorrencia> Ocorrencias { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Presenca> Presencas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chaves primárias compostas das tabelas associativas do sql_V1.sql
            modelBuilder.Entity<PessoaPapel>()
                .HasKey(pp => new { pp.PessoaId, pp.PapelId });

            modelBuilder.Entity<EventoResponsavel>()
                .HasKey(er => new { er.EventoId, er.ResponsavelId });

            // Relacionamentos 1:1 com Pessoa
            modelBuilder.Entity<Pessoa>()
                .HasOne(p => p.Endereco)
                .WithOne(e => e.Pessoa)
                .HasForeignKey<Endereco>(e => e.PessoaId);

            modelBuilder.Entity<Pessoa>()
                .HasOne(p => p.UsuarioInterno)
                .WithOne(u => u.Pessoa)
                .HasForeignKey<UsuarioInterno>(u => u.PessoaId);

            modelBuilder.Entity<Pessoa>()
                .HasOne(p => p.Responsavel)
                .WithOne(r => r.Pessoa)
                .HasForeignKey<Responsavel>(r => r.PessoaId);

            // Índices únicos do sql_V1.sql
            modelBuilder.Entity<Pessoa>()
                .HasIndex(p => p.Cpf)
                .IsUnique();

            modelBuilder.Entity<Papel>()
                .HasIndex(p => p.NomePapel)
                .IsUnique();

            modelBuilder.Entity<UsuarioInterno>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Inscricao>()
                .HasIndex(i => i.CodigoInscricao)
                .IsUnique();

            modelBuilder.Entity<Inscricao>()
                .HasIndex(i => new { i.PessoaId, i.EventoId })
                .IsUnique();

            modelBuilder.Entity<Presenca>()
                .HasIndex(p => new { p.InscricaoId, p.OcorrenciaId })
                .IsUnique();
        }
    }
}
