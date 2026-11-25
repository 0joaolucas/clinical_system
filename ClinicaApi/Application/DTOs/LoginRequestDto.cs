namespace ClinicaApi.Application.DTOs.Auth;

public class LoginRequestDTO
{
    public string Cpf { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}