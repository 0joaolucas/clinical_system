using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClinicaApi.Domain.Entities;

public class Consulta
{
    [Key]
    public int IdConsulta { get; set; }

    [ForeignKey(nameof(Medico))]
    public int IdPaciente { get; set; }
    public Paciente? Paciente { get; set; }

    [ForeignKey(nameof(Paciente))]
    public int IdMedico { get; set; }
    public Medico? Medico { get; set; }
    public DateOnly DataConsulta {get; set;}
    public TimeOnly HoraConsulta{get; set;}
    public string Observacao {get; set;} = string.Empty;
    public string TipoConsulta {get; set;} = string.Empty;


}