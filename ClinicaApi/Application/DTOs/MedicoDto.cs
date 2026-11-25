namespace ClinicaApi.Application.DTOs;

public class MedicoDto
{
    public int IdMedico { get; set; }
    public string Nome {get; set;} = string.Empty;
    public string CRM {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
    public string Especialidade {get; set;} = string.Empty;
    public string Telefone {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Funcao {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
}