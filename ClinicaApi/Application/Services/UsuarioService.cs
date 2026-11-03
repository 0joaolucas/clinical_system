
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicaApi.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "ListaUsuarios";

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IMemoryCache cache)
    {
        _usuarioRepository = usuarioRepository;
        _cache = cache;
    }

    public async Task<ResultWrapper<IEnumerable<Usuario>>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<Usuario>? usuarios)
            || usuarios == null)
        {
            usuarios = await _usuarioRepository.GetAllAsync();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheKey, usuarios, options);
        }

        return Sucesso(usuarios);
    }

    public async Task<ResultWrapper<Usuario>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Erro<Usuario>("ID inválido.");

        var usuario = await _usuarioRepository.GetByIdAsync(id);

        if (usuario == null)
            return Erro<Usuario>("Usuário não encontrado.");

        return Sucesso(usuario);
    }

    public async Task<ResultWrapper<Usuario>> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Erro<Usuario>("E-mail é obrigatório.");

        var usuario = await _usuarioRepository.GetByEmailAsync(email);

        if (usuario == null)
            return Erro<Usuario>("Usuário não encontrado.");

        return Sucesso(usuario);
    }

    public async Task<ResultWrapper<bool>> AddAsync(Usuario usuario)
    {
        if (usuario == null)
            return Erro<bool>("Usuário é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(usuario.Nome))
            return Erro<bool>("Nome é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(usuario.Email))
            return Erro<bool>("E-mail é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(usuario.Senha))
            return Erro<bool>("Senha é obrigatória.", false);

        if (string.IsNullOrWhiteSpace(usuario.Perfil))
            return Erro<bool>("Perfil é obrigatória.", false);

        var existente =
            await _usuarioRepository.GetByEmailAsync(usuario.Email);

        if (existente != null)
            return Erro<bool>("Já existe um usuário cadastrado com este e-mail.", false);

        await _usuarioRepository.AddAsync(usuario);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Usuário cadastrado com sucesso.");
    }

    public async Task<ResultWrapper<bool>> UpdateAsync(Usuario usuario)
    {
        if (usuario == null)
            return Erro<bool>("Usuário é obrigatório.", false);

        if (usuario.Id <= 0)
            return Erro<bool>("ID inválido.", false);

        if (string.IsNullOrWhiteSpace(usuario.Nome))
            return Erro<bool>("Nome é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(usuario.Email))
            return Erro<bool>("E-mail é obrigatório.", false);

        if (string.IsNullOrWhiteSpace(usuario.Perfil))
            return Erro<bool>("Função é obrigatória.", false);

        var existente =
            await _usuarioRepository.GetByIdAsync(usuario.Id);

        if (existente == null)
            return Erro<bool>("Usuário não encontrado.", false);

        var porEmail =
            await _usuarioRepository.GetByEmailAsync(usuario.Email);

        if (porEmail != null && porEmail.Id != usuario.Id)
            return Erro<bool>("Já existe outro usuário cadastrado com este e-mail.", false);

        await _usuarioRepository.UpdateAsync(usuario);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Usuário atualizado com sucesso.");
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return Erro<bool>("ID inválido.", false);

        var usuario = await _usuarioRepository.GetByIdAsync(id);

        if (usuario == null)
            return Erro<bool>("Usuário não encontrado.", false);

        await _usuarioRepository.DeleteAsync(id);

        _cache.Remove(CacheKey);

        return Sucesso(true, "Usuário excluído com sucesso.");
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
