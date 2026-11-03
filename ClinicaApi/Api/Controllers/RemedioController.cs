using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemedioController : ControllerBase
{
    private readonly IRemedioService _remedioService;

    public RemedioController(IRemedioService remedioService)
    {
        _remedioService = remedioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _remedioService.GetAllAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await _remedioService.GetByIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("nome/{nome}")]
    public async Task<IActionResult> GetByName(string nome)
    {
        var resultado = await _remedioService.GetByNomeAsync(nome);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Remedio remedio)
    {
        var resultado = await _remedioService.AddAsync(remedio);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Remedio remedio)
    {
        var resultado = await _remedioService.UpdateAsync(remedio);

        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _remedioService.DeleteAsync(id);

        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}