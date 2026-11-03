using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacaoController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacaoController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _transacaoService.GetAllAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await _transacaoService.GetByIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Transacao transacao)
    {
        var resultado = await _transacaoService.AddAsync(transacao);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Transacao transacao)
    {
        var resultado = await _transacaoService.UpdateAsync(transacao);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _transacaoService.DeleteAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}