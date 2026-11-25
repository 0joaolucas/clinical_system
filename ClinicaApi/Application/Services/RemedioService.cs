
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Application.Common;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicaApi.Application.Services;

public class RemedioService : IRemedioService
{
    private readonly IRemedioRepository _remedioRepository;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "ListaRemedios";

    public RemedioService(
        IRemedioRepository remedioRepository,
        IMemoryCache cache)
    {
        _remedioRepository = remedioRepository;
        _cache = cache;
    }

    public async Task<ResultWrapper<IEnumerable<Remedio>>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<Remedio>? remedios)
            || remedios == null)
        {
            remedios = await _remedioRepository.GetAllAsync();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheKey, remedios, options);
        }

        return Sucesso(remedios);
    }

    public async Task<ResultWrapper<Remedio>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Erro<Remedio>("ID inválido.");

        var remedio = await _remedioRepository.GetByIdAsync(id);

        if (remedio == null)
            return Erro<Remedio>("Remédio não encontrado.");

        return Sucesso(remedio);
    }

    public async Task<ResultWrapper<Remedio>> GetByNomeAsync(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Erro<Remedio>("Nome do remédio é obrigatório.");

        var remedio = await _remedioRepository.GetByNameAsync(nome);

        if (remedio == null)
            return Erro<Remedio>("Remédio não encontrado.");

        return Sucesso(remedio);
    }

    public async Task<ResultWrapper<bool>> AddAsync(Remedio remedio)
    {
        if (remedio == null)
            return Erro<bool>("Remédio é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(remedio.Nome))
            return Erro<bool>("Nome do remédio é obrigatório.", false);

        var existente =
            await _remedioRepository.GetByNameAsync(remedio.Nome);

        if (existente != null)
            return Erro<bool>("Já existe um remédio cadastrado com este nome.", false);

        await _remedioRepository.AddAsync(remedio);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Remédio cadastrado com sucesso.");
    }

    public async Task<ResultWrapper<bool>> UpdateAsync(Remedio remedio)
    {
        if (remedio == null)
            return Erro<bool>("Remédio é obrigatório.", false);

        if (remedio.Id <= 0)
            return Erro<bool>("ID inválido.", false);

        if (string.IsNullOrWhiteSpace(remedio.Nome))
            return Erro<bool>("Nome do remédio é obrigatório.", false);

        var existente = await _remedioRepository.GetByIdAsync(remedio.Id);

        if (existente == null)
            return Erro<bool>("Remédio não encontrado.", false);

        var porNome = await _remedioRepository.GetByNameAsync(remedio.Nome);

        if (porNome != null && porNome.Id != remedio.Id)
            return Erro<bool>("Já existe outro remédio cadastrado com este nome.", false);

        existente.Nome = remedio.Nome;
        existente.Quantidade = remedio.Quantidade;

        await _remedioRepository.UpdateAsync(existente);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Remédio atualizado com sucesso.");
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return Erro<bool>("ID inválido.", false);

        var remedio =
            await _remedioRepository.GetByIdAsync(id);

        if (remedio == null)
            return Erro<bool>("Remédio não encontrado.", false);

        await _remedioRepository.DeleteAsync(id);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Remédio excluído com sucesso.");
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
