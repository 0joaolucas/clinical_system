namespace ClinicaApi.Application.DTOs;

public class LoginDto
{
    public string Cpf {get; set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;
    public string Funcao {get; set;} = string.Empty;
}