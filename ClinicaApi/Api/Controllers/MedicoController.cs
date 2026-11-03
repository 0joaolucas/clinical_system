using ClinicaApi.Application.DTOs;
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicoController : ControllerBase
{
    private readonly IMedicoService _medicoService;

    public MedicoController(IMedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _medicoService.GetAllAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await _medicoService.GetByIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var resultado = await _medicoService.GetByCpfAsync(cpf);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("crm/{crm}")]
    public async Task<IActionResult> GetByCrm(string crm)
    {
        var resultado = await _medicoService.GetByCrmAsync(crm);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]MedicoDto medicoDto)
    {
        var resultado = await _medicoService.AddAsync(medicoDto);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut]
    public async Task<IActionResult> Update(MedicoDto medicoDto)
    {
        var resultado = await _medicoService.UpdateAsync(medicoDto);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _medicoService.DeleteAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}