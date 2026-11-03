using ClinicaApi.Domain.Entities;

namespace ClinicaApi.Application.DTOs;

public class TransacaoDto
{
    public int Id {get; set;}
    public Paciente? Paciente {get; set;}
    public Remedio? Remedio {get; set;}
    public int Quantidade {get; set;}
    public decimal Valor {get; set;}
    public DateOnly DataTransacao {get; set;}
}
