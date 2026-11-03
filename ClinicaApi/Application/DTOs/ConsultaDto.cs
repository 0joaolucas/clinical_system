namespace ClinicaApi.Application.DTOs;

using ClinicaApi.Domain.Entities;

public class ConsultaDto
{
    public int IdConsulta{get; set;}
    public Paciente? Paciente {get; set;}
    public Medico? Medico {get; set;}
    public DateOnly DataConsulta {get; set;}
    public TimeOnly HoraConsulta {get; set;}
    public string Observacao {get; set;} = string.Empty;
    public string TipoConsulta {get; set;} = string.Empty;
}