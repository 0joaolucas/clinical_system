
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Application.Common;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicaApi.Application.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IRemedioRepository _remedioRepository;
    private readonly IMemoryCache _cache;

    public TransacaoService(
        ITransacaoRepository transacaoRepository,
        IPacienteRepository pacienteRepository,
        IRemedioRepository remedioRepository,
        IMemoryCache cache)
    {
        _transacaoRepository = transacaoRepository;
        _pacienteRepository = pacienteRepository;
        _remedioRepository = remedioRepository;
        _cache = cache;
    }

    public async Task<ResultWrapper<IEnumerable<Transacao>>> GetAllAsync()
    {
        var transacoes = await _transacaoRepository.GetAllAsync();

        return Sucesso(transacoes);
    }

    public async Task<ResultWrapper<Transacao>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Erro<Transacao>("ID inválido.");

        var transacao =
            await _transacaoRepository.GetByIdAsync(id);

        if (transacao == null)
            return Erro<Transacao>("Transação não encontrada.");

        return Sucesso(transacao);
    }

    public async Task<ResultWrapper<IEnumerable<Transacao>>> GetByPacienteAsync(
        int idPaciente)
    {
        if (idPaciente <= 0)
            return Erro<IEnumerable<Transacao>>(
                "ID do paciente inválido.");

        var paciente =
            await _pacienteRepository.GetByIdAsync(idPaciente);

        if (paciente == null)
            return Erro<IEnumerable<Transacao>>(
                "Paciente não encontrado.");

        var transacoes =
            await _transacaoRepository.GetByPacienteAsync(idPaciente);

        return Sucesso(transacoes);
    }

    public async Task<ResultWrapper<IEnumerable<Transacao>>> GetByRemedioAsync(
        int idRemedio)
    {
        if (idRemedio <= 0)
            return Erro<IEnumerable<Transacao>>(
                "ID do remédio inválido.");

        var remedio =
            await _remedioRepository.GetByIdAsync(idRemedio);

        if (remedio == null)
            return Erro<IEnumerable<Transacao>>(
                "Remédio não encontrado.");

        var transacoes =
            await _transacaoRepository.GetByRemedioAsync(idRemedio);

        return Sucesso(transacoes);
    }

    public async Task<ResultWrapper<bool>> AddAsync(Transacao transacao)
    {
        if (transacao == null)
            return Erro<bool>("Transação é obrigatória.", false);

        if (transacao.IdPaciente <= 0)
            return Erro<bool>("ID do paciente inválido.", false);

        if (transacao.IdRemedio <= 0)
            return Erro<bool>("ID do remédio inválido.", false);

        if (transacao.Quantidade <= 0)
            return Erro<bool>("A quantidade deve ser maior que zero.", false);

        var paciente =
            await _pacienteRepository.GetByIdAsync(transacao.IdPaciente);

        if (paciente == null)
            return Erro<bool>("Paciente não encontrado.", false);

        var remedio =
            await _remedioRepository.GetByIdAsync(transacao.IdRemedio);

        if (remedio == null)
            return Erro<bool>("Remédio não encontrado.", false);

        if (remedio.Quantidade < transacao.Quantidade)
            return Erro<bool>("Estoque insuficiente.", false);

        remedio.Quantidade -= transacao.Quantidade;

        await _remedioRepository.UpdateAsync(remedio);
        await _transacaoRepository.AddAsync(transacao);

        // Remove o cache porque o estoque foi alterado.
        _cache.Remove("ListaRemedios");

        return Sucesso(true, "Transação cadastrada com sucesso.");
    }

    public async Task<ResultWrapper<bool>> UpdateAsync(Transacao transacao)
    {
        if (transacao == null)
            return Erro<bool>("Transação é obrigatória.", false);

        if (transacao.Id <= 0)
            return Erro<bool>("ID inválido.", false);

        if (transacao.IdPaciente <= 0)
            return Erro<bool>("ID do paciente inválido.", false);

        if (transacao.IdRemedio <= 0)
            return Erro<bool>("ID do remédio inválido.", false);

        if (transacao.Quantidade <= 0)
            return Erro<bool>(
                "A quantidade deve ser maior que zero.",
                false);

        var existente =
            await _transacaoRepository.GetByIdAsync(transacao.Id);

        if (existente == null)
            return Erro<bool>("Transação não encontrada.", false);

        var paciente =
            await _pacienteRepository.GetByIdAsync(transacao.IdPaciente);

        if (paciente == null)
            return Erro<bool>("Paciente não encontrado.", false);

        var remedio =
            await _remedioRepository.GetByIdAsync(transacao.IdRemedio);

        if (remedio == null)
            return Erro<bool>("Remédio não encontrado.", false);

        // Atualiza os campos da entidade que já está sendo rastreada
        existente.IdPaciente = transacao.IdPaciente;
        existente.IdRemedio = transacao.IdRemedio;
        existente.Quantidade = transacao.Quantidade;
        existente.Valor = transacao.Valor;
        existente.DataTransacao = transacao.DataTransacao;

        await _transacaoRepository.UpdateAsync(existente);

        return Sucesso(true, "Transação atualizada com sucesso.");
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return Erro<bool>("ID inválido.", false);

        var transacao =
            await _transacaoRepository.GetByIdAsync(id);

        if (transacao == null)
            return Erro<bool>("Transação não encontrada.", false);

        await _transacaoRepository.DeleteAsync(id);

        return Sucesso(true, "Transação excluída com sucesso.");
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
