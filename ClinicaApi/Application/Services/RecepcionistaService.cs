namespace ClinicaApi.Application.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using ClinicaApi.Application.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class RecepcionistaService : IRecepcionistaService
{
    private readonly IRecepcionistaRepository _recepcionistaRepository;
    private readonly ILoginRepository _loginRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<Login> _passwordHasher;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "ListaRecepcionistas";

    public RecepcionistaService(IRecepcionistaRepository recepcionistaRepository, ILoginRepository loginRepository, IUnitOfWork unitOfWork, IPasswordHasher<Login> passwordHasher, IMemoryCache cache)
    {
        _recepcionistaRepository = recepcionistaRepository;
        _loginRepository = loginRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _cache = cache;
    }

    public async Task<ResultWrapper<IEnumerable<Recepcionista>>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<Recepcionista>? recepcionistas ))
        {
            recepcionistas = await _recepcionistaRepository.GetAllAsync();

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheKey, recepcionistas, options);
        }

        return Sucesso(recepcionistas);
    }

    public async Task<ResultWrapper<Recepcionista>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return Erro<Recepcionista>("ID inválido.");
        }

        var recepcionista = await _recepcionistaRepository.GetByIdAsync(id);

        if (recepcionista == null)
        {
            return Erro<Recepcionista>("Recepcionista não encontrado.");
        }

        return Sucesso(recepcionista);
    }

    public async Task<ResultWrapper<Recepcionista>> GetByCpfAsync(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return Erro<Recepcionista>("CPF é obrigatório.");
        }

        var recepcionista =
            await _recepcionistaRepository.GetByCpfAsync(cpf);

        if (recepcionista == null)
        {
            return Erro<Recepcionista>("Recepcionista não encontrado.");
        }

        return Sucesso(recepcionista);
    }

    public async Task<ResultWrapper<bool>> AddAsync(RecepcionistaDto recepcionistaDto)
    {
        if (recepcionistaDto == null)
            return Erro<bool>("Recepcionista é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(recepcionistaDto.Nome))
            return Erro<bool>("Nome do recepcionista é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(recepcionistaDto.Cpf))
            return Erro<bool>("CPF é obrigatório.", false);

        var existente = await _recepcionistaRepository.GetByCpfAsync(recepcionistaDto.Cpf);

        if (existente != null)
            return Erro<bool>("Já existe um recepcionista cadastrado com este CPF.", false);
        
        var loginExistente = await _loginRepository.GetByCpfAsync(recepcionistaDto.Cpf);

        if(loginExistente != null)
            return Erro<bool>("Já existe um login cadastrado com este CPF.", false);

        var recepcionista = new Recepcionista
        {
            Cpf = recepcionistaDto.Cpf,
            Nome = recepcionistaDto.Nome,
            Telefone = recepcionistaDto.Telefone,
            Email = recepcionistaDto.Email
        };

        var login = new Login
        {
            Cpf = recepcionistaDto.Cpf,
            Nome = recepcionistaDto.Nome,
            Funcao = "Recepcionista"
        };

        login.Senha = _passwordHasher.HashPassword(login, recepcionistaDto.Senha
        );

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _recepcionistaRepository.AddAsync(recepcionista);
            await _loginRepository.AddAsync(login);

            await _unitOfWork.CommitAsync();
            _cache.Remove(CacheKey);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        return Sucesso(true, "Recepcionista cadastrado com sucesso.");
    }

    public async Task<ResultWrapper<bool>> UpdateAsync(
        Recepcionista recepcionista)
    {
        if (recepcionista == null)
            return Erro<bool>("Recepcionista é obrigatório.", false);

        if (recepcionista.Id <= 0)
            return Erro<bool>("ID inválido.", false);

        if (string.IsNullOrWhiteSpace(recepcionista.Nome))
            return Erro<bool>("Nome do recepcionista é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(recepcionista.Cpf))
            return Erro<bool>("CPF é obrigatório.", false);

        var existente = await _recepcionistaRepository.GetByIdAsync(recepcionista.Id);

        if (existente == null)
            return Erro<bool>("Recepcionista não encontrado.", false);

        var porCpf =await _recepcionistaRepository.GetByCpfAsync(recepcionista.Cpf);

        if (porCpf != null && porCpf.Id != recepcionista.Id)
            return Erro<bool>("Já existe outro recepcionista cadastrado com este CPF.",
                false);

        existente.Cpf = recepcionista.Cpf;
        existente.Email = recepcionista.Email;
        existente.Nome = recepcionista.Nome;
        existente.Telefone = recepcionista.Telefone;

        await _recepcionistaRepository.UpdateAsync(recepcionista);
        _cache.Remove(CacheKey);

        return Sucesso(true, "Recepcionista atualizado com sucesso.");
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return Erro<bool>("ID inválido.", false);

        var recepcionista =
            await _recepcionistaRepository.GetByIdAsync(id);

        if (recepcionista == null)
            return Erro<bool>("Recepcionista não encontrado.", false);

        await _recepcionistaRepository.DeleteAsync(id);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Recepcionista excluído com sucesso.");
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

    private ResultWrapper<T> Sucesso<T>(T data, string mensagem = "Operação realizada com sucesso.")
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
