using ClinicaApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] string cpf, [FromQuery] string senha)
    {
        var resultado = await _loginService.LoginAsync(cpf, senha);

        if(!resultado.Success)
            return Unauthorized(resultado);

        return Ok(resultado);
    }
}
