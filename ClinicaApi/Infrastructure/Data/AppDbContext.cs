using Microsoft.EntityFrameworkCore;
using ClinicaApi.Domain.Entities;

namespace ClinicaApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Usuario> Usuarios {get; set;}
    public DbSet<Medico> Medicos {get; set;}
    public DbSet<Paciente> Pacientes {get; set;}
    public DbSet<Recepcionista> Recepcionistas {get; set;}
    public DbSet<Remedio> Remedios {get; set;}
    public DbSet<Transacao> Transacoes {get; set;}
    public DbSet<Consulta> Consultas {get; set;}
    public DbSet<Login> Logins {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transacao>()
            .Property(t => t.Valor)
            .HasPrecision(10, 2);
    }
}