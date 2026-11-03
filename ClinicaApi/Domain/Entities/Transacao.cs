using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaApi.Domain.Entities;

public class Transacao
{
    public int Id {get; set;}

    [ForeignKey(nameof(Paciente))]
    public int IdPaciente { get; set; }
    public Paciente? Paciente { get; set; }

    [ForeignKey(nameof(Remedio))]
    public int IdRemedio { get; set; }
    public Remedio? Remedio { get; set; }

    public int Quantidade {get; set;}

    public decimal Valor {get; set;}
    
    public DateOnly DataTransacao {get; set;}
}