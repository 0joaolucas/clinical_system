using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacienteController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _pacienteService.GetAllAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await _pacienteService.GetByIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var resultado = await _pacienteService.GetByCpfAsync(cpf);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Paciente paciente)
    {
        var resultado = await _pacienteService.AddAsync(paciente);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Paciente paciente)
    {
        var resultado = await _pacienteService.UpdateAsync(paciente);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _pacienteService.DeleteAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}