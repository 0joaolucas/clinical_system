namespace ClinicaApi.Domain.Entities;

public class Login
{
    public int Id {get; set;}
    public string Cpf {get; set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
    public string Funcao {get; set;} = string.Empty;
}