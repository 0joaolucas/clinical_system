using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _usuarioService.GetAllAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await _usuarioService.GetByIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }


    [HttpPost]
    public async Task<IActionResult> Add(Usuario usuario)
    {
        var resultado = await _usuarioService.AddAsync(usuario);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Usuario usuario)
    {
        var resultado = await _usuarioService.UpdateAsync(usuario);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _usuarioService.DeleteAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}