using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var resultado = await _loginService.LoginAsync(
            request.Cpf,
            request.Senha
        );

        if (!resultado.Success)
            return Unauthorized(resultado);

        return Ok(resultado);
    }
}