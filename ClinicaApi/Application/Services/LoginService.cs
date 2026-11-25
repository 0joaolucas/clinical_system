using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace ClinicaApi.Application.Services;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<Login> _passwordHasher;

    public LoginService(
        ILoginRepository loginRepository,
        IConfiguration configuration,
        IPasswordHasher<Login> passwordHasher)
    {
        _loginRepository = loginRepository;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResultWrapper<string>> LoginAsync(string cpf, string senha)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return Erro("CPF é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(senha))
        {
            return Erro("Senha é obrigatória.");
        }

        var login = await _loginRepository.GetByCpfAsync(cpf);

        if (login == null)
        {
            return Erro("CPF ou senha inválidos.");
        }

        var verificacao = _passwordHasher.VerifyHashedPassword(
            login,
            login.Senha,
            senha
        );

        if (verificacao == PasswordVerificationResult.Failed)
        {
            return Erro("CPF ou senha inválidos.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
            new Claim(ClaimTypes.Role, login.Funcao)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpirationMinutes"]!)
            ),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new ResultWrapper<string>
        {
            Success = true,
            Message = "Login realizado com sucesso.",
            Data = tokenString,
            Errors = new List<string>()
        };
    }

    private ResultWrapper<string> Erro(string mensagem)
    {
        return new ResultWrapper<string>
        {
            Success = false,
            Message = "Não foi possível realizar o login.",
            Data = null,
            Errors = new List<string> { mensagem }
        };
    }
}