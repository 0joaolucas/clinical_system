
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicaApi.Application.Services;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _consultaRepository;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "ListaConsultas";

    public ConsultaService(
        IConsultaRepository consultaRepository,
        IMedicoRepository medicoRepository,
        IPacienteRepository pacienteRepository,
        IMemoryCache cache)
    {
        _consultaRepository = consultaRepository;
        _medicoRepository = medicoRepository;
        _pacienteRepository = pacienteRepository;
        _cache = cache;
    }

    public async Task<ResultWrapper<IEnumerable<Consulta>>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<Consulta>? consultas)
            || consultas == null)
        {
            consultas = await _consultaRepository.GetAllAsync();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheKey, consultas, options);
        }

        return Sucesso(consultas);
    }

    public async Task<ResultWrapper<Consulta>> GetByIdAsync(int id)
    {
        if (id < 0)
            return Erro<Consulta>("ID inválido.");

        var consulta = await _consultaRepository.GetByIdAsync(id);

        if (consulta == null)
            return Erro<Consulta>("Consulta não encontrada.");

        return Sucesso(consulta);
    }

    public async Task<ResultWrapper<IEnumerable<Consulta>>> GetByMedicoAsync(
        int idMedico)
    {
        if (idMedico < 0)
            return Erro<IEnumerable<Consulta>>("ID do médico inválido.");

        var medico = await _medicoRepository.GetByIdAsync(idMedico);

        if (medico == null)
            return Erro<IEnumerable<Consulta>>("Médico não encontrado.");

        var consultas =
            await _consultaRepository.GetByMedicoAsync(idMedico);

        return Sucesso(consultas);
    }

    public async Task<ResultWrapper<IEnumerable<Consulta>>> GetByPacienteAsync(
        int idPaciente)
    {
        if (idPaciente < 0)
            return Erro<IEnumerable<Consulta>>("ID do paciente inválido.");

        var paciente =
            await _pacienteRepository.GetByIdAsync(idPaciente);

        if (paciente == null)
            return Erro<IEnumerable<Consulta>>("Paciente não encontrado.");

        var consultas =
            await _consultaRepository.GetByPacienteAsync(idPaciente);

        return Sucesso(consultas);
    }

    public async Task<ResultWrapper<bool>> AddAsync(Consulta consulta)
    {
        if (consulta == null)
            return Erro<bool>("Consulta é obrigatória.", false);

        if (consulta.IdMedico < 0)
            return Erro<bool>("ID do médico inválido.", false);

        if (consulta.IdPaciente < 0)
            return Erro<bool>("ID do paciente inválido.", false);

        var medico =
            await _medicoRepository.GetByIdAsync(consulta.IdMedico);

        if (medico == null)
            return Erro<bool>("Médico não encontrado.", false);

        var paciente =
            await _pacienteRepository.GetByIdAsync(consulta.IdPaciente);

        if (paciente == null)
            return Erro<bool>("Paciente não encontrado.", false);

        await _consultaRepository.AddAsync(consulta);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Consulta cadastrada com sucesso.");
    }

    public async Task<ResultWrapper<bool>> UpdateAsync(Consulta consulta)
    {
        if (consulta == null)
            return Erro<bool>("Consulta é obrigatória.", false);

        if (consulta.IdConsulta < 0)
            return Erro<bool>("ID inválido.", false);

        if (consulta.IdMedico < 0)
            return Erro<bool>("ID do médico inválido.", false);

        if (consulta.IdPaciente < 0)
            return Erro<bool>("ID do paciente inválido.", false);

        var existente =
            await _consultaRepository.GetByIdAsync(consulta.IdConsulta);

        if (existente == null)
            return Erro<bool>("Consulta não encontrada.", false);

        var medico =
            await _medicoRepository.GetByIdAsync(consulta.IdMedico);

        if (medico == null)
            return Erro<bool>("Médico não encontrado.", false);

        var paciente =
            await _pacienteRepository.GetByIdAsync(consulta.IdPaciente);

        if (paciente == null)
            return Erro<bool>("Paciente não encontrado.", false);

        existente.IdPaciente = consulta.IdPaciente;
        existente.IdMedico = consulta.IdMedico;
        existente.DataConsulta = consulta.DataConsulta;
        existente.HoraConsulta = consulta.HoraConsulta;
        existente.TipoConsulta = consulta.TipoConsulta;
        existente.Observacao = consulta.Observacao;

        await _consultaRepository.UpdateAsync(existente);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Consulta atualizada com sucesso.");
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id < 0)
            return Erro<bool>("ID inválido.", false);

        var consulta =
            await _consultaRepository.GetByIdAsync(id);

        if (consulta == null)
            return Erro<bool>("Consulta não encontrada.", false);

        await _consultaRepository.DeleteAsync(id);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Consulta excluída com sucesso.");
    }

    private ResultWrapper<T> Erro<T>(string mensagem, T? data = default)
    {
        return new ResultWrapper<T>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = data,
            Errors = new List<string> { mensagem }
        };
    }

    private ResultWrapper<T> Sucesso<T>(
        T data,
        string mensagem = "Operação realizada com sucesso.")
    {
        return new ResultWrapper<T>
        {
            Success = true,
            Message = mensagem,
            Data = data,
            Errors = new List<string>()
        };
    }
}
