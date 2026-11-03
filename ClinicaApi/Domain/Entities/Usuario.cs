using System.Runtime.CompilerServices;

namespace ClinicaApi.Domain.Entities;

public class Usuario
{
    public int Id {get; set;}
    public string Cpf {get; set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;

    public string Email {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
    public string Perfil {get; set;} = string.Empty;
}