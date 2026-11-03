using ClinicaApi.Application.DTOs;
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecepcionistaController : ControllerBase
{
    private readonly IRecepcionistaService _recepcionistaService;

    public RecepcionistaController(IRecepcionistaService recepcionistaService)
    {
        _recepcionistaService = recepcionistaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _recepcionistaService.GetAllAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await _recepcionistaService.GetByIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var resultado = await _recepcionistaService.GetByCpfAsync(cpf);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RecepcionistaDto recepcionistaDto)
    {
        var resultado = await _recepcionistaService.AddAsync(recepcionistaDto);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Recepcionista recepcionista)
    {
        var resultado = await _recepcionistaService.UpdateAsync(recepcionista);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _recepcionistaService.DeleteAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}