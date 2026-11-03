namespace ClinicaApi.Application.DTOs;

public class UsuarioDto
{
    public int Id {get; set;}
    public string Cpf {get; set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;
    public string Perfil {get; set;} = string.Empty;
}