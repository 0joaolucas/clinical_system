namespace ClinicaApi.Application.DTOs;

public class PacienteDto
{
    public string Cpf {get; set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;
    public DateOnly DataNasc {get; set;}
    public string Telefone {get; set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public string Cidade {get; set;} = string.Empty;
    public string Bairro {get; set;} = string.Empty;
    public string Rua {get; set;} = string.Empty;
    public string Plano {get; set;} = string.Empty;
    public string Historico {get; set;} = string.Empty;
}