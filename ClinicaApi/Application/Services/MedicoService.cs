
using ClinicaApi.Application.Common;
using ClinicaApi.Application.DTOs;
using ClinicaApi.Application.Interfaces;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicaApi.Application.Services;

public class MedicoService : IMedicoService
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly ILoginRepository _loginRepository;
    private readonly IPasswordHasher<Login> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "ListaMedicos";

    public MedicoService(
        IMedicoRepository medicoRepository,
        ILoginRepository loginRepository,
        IPasswordHasher<Login> passwordHasher,
        IUnitOfWork unitOfWork,
        IMemoryCache cache)
    {
        _medicoRepository = medicoRepository;
        _loginRepository = loginRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<ResultWrapper<bool>> AddAsync(MedicoDto medicoDto)
    {
        if (medicoDto == null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Médico é obrigatório."
                }
            };
        }

        var medico = new Medico
        {
            Cpf = medicoDto.Cpf,
            Nome = medicoDto.Nome,
            Crm = medicoDto.CRM,
            Especialidade = medicoDto.Especialidade,
            Telefone = medicoDto.Telefone,
            Email = medicoDto.Email,
            Funcao = "Medico"
        };

        if (string.IsNullOrWhiteSpace(medico.Nome))
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Nome do médico é obrigatório."
                }
            };
        }

        if (string.IsNullOrWhiteSpace(medico.Cpf))
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "CPF é obrigatório."
                }
            };
        }

        if (string.IsNullOrWhiteSpace(medico.Crm))
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "CRM é obrigatório."
                }
            };
        }

        var loginExistente = await _loginRepository.GetByCpfAsync(medico.Cpf);

        if (loginExistente != null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Já existe um login cadastrado com este CPF."
                }
            };
        }

        var medicoPorCpf = await _medicoRepository.GetByCpfAsync(medico.Cpf);

        if (medicoPorCpf != null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Já existe um médico cadastrado com este CPF."
                }
            };
        }

        var medicoPorCrm =
            await _medicoRepository.GetByCrmAsync(medico.Crm);

        if (medicoPorCrm != null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Já existe um médico cadastrado com este CRM."
                }
            };
        }

        if (string.IsNullOrWhiteSpace(medicoDto.Senha))
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Senha é obrigatória"
                }
            };
        }

        var login = new Login
        {
            Cpf = medico.Cpf,
            Nome = medico.Nome,
            Funcao = "Medico"
        };

        login.Senha = _passwordHasher.HashPassword(login, medicoDto.Senha);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Cadastra o médico
            await _medicoRepository.AddAsync(medico);

            // Cadastra o login
            await _loginRepository.AddAsync(login);

            // Confirma as duas operações
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            // Desfaz as alterações em caso de erro
            await _unitOfWork.RollbackAsync();
            throw;
        }

        _cache.Remove(CacheKey);

        return new ResultWrapper<bool>
        {
            Success = true,
            Message = "Médico e login cadastrados com sucesso.",
            Data = true,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id < 0)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "ID inválido."
                }
            };
        }

        var medico =
            await _medicoRepository.GetByIdAsync(id);

        if (medico == null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Médico não encontrado."
                }
            };
        }

        await _medicoRepository.DeleteAsync(id);

        _cache.Remove(CacheKey);

        return new ResultWrapper<bool>
        {
            Success = true,
            Message = "Médico excluído com sucesso.",
            Data = true,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<IEnumerable<Medico>>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<Medico>? medicos)
            || medicos == null)
        {
            medicos = await _medicoRepository.GetAllAsync();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheKey, medicos, options);
        }

        return new ResultWrapper<IEnumerable<Medico>>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = medicos,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<Medico>> GetByCpfAsync(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return new ResultWrapper<Medico>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "CPF é obrigatório."
                }
            };
        }

        var medico = await _medicoRepository.GetByCpfAsync(cpf);

        if (medico == null)
        {
            return new ResultWrapper<Medico>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "Médico não encontrado."
                }
            };
        }

        return new ResultWrapper<Medico>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = medico,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<Medico>> GetByCrmAsync(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
        {
            return new ResultWrapper<Medico>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "CRM é obrigatório."
                }
            };
        }

        var medico = await _medicoRepository.GetByCrmAsync(crm);

        if (medico == null)
        {
            return new ResultWrapper<Medico>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "Médico não encontrado."
                }
            };
        }

        return new ResultWrapper<Medico>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = medico,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<Medico>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return new ResultWrapper<Medico>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "ID inválido."
                }
            };
        }

        var medico = await _medicoRepository.GetByIdAsync(id);

        if (medico == null)
        {
            return new ResultWrapper<Medico>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "Médico não encontrado."
                }
            };
        }

        return new ResultWrapper<Medico>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = medico,
            Errors = new List<string>()
        };
    }


    public async Task<ResultWrapper<bool>> UpdateAsync(MedicoDto medicoDto)
{
    if (medicoDto == null)
    {
        return new ResultWrapper<bool>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = false,
            Errors = new List<string> { "Médico é obrigatório." }
        };
    }

    if (medicoDto.IdMedico <= 0)
    {
        return new ResultWrapper<bool>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = false,
            Errors = new List<string> { "ID inválido." }
        };
    }

    if (string.IsNullOrWhiteSpace(medicoDto.Nome) ||
        string.IsNullOrWhiteSpace(medicoDto.Cpf) ||
        string.IsNullOrWhiteSpace(medicoDto.CRM))
    {
        return new ResultWrapper<bool>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = false,
            Errors = new List<string> { "Nome, CPF e CRM são obrigatórios." }
        };
    }

    // Busca o médico que será atualizado
    var medicoExistente = await _medicoRepository.GetByIdAsync(medicoDto.IdMedico);

    if (medicoExistente == null)
    {
        return new ResultWrapper<bool>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = false,
            Errors = new List<string> { "Médico não encontrado." }
        };
    }

    // Verifica se o CPF já pertence a outro médico
    var medicoPorCpf = await _medicoRepository.GetByCpfAsync(medicoDto.Cpf);

    if (medicoPorCpf != null && medicoPorCpf.IdMedico != medicoDto.IdMedico)
    {
        return new ResultWrapper<bool>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = false,
            Errors = new List<string> { "Já existe outro médico cadastrado com este CPF." }
        };
    }

    // Verifica se o CRM já pertence a outro médico
    var medicoPorCrm = await _medicoRepository.GetByCrmAsync(medicoDto.CRM);

    if (medicoPorCrm != null && medicoPorCrm.IdMedico != medicoDto.IdMedico)
    {
        return new ResultWrapper<bool>
        {
            Success = false,
            Message = "Não foi possível realizar a operação.",
            Data = false,
            Errors = new List<string> { "Já existe outro médico cadastrado com este CRM." }
        };
    }

    // Atualiza a própria entidade que já está sendo rastreada
    medicoExistente.Nome = medicoDto.Nome;
    medicoExistente.Cpf = medicoDto.Cpf;
    medicoExistente.Crm = medicoDto.CRM;
    medicoExistente.Especialidade = medicoDto.Especialidade;
    medicoExistente.Telefone = medicoDto.Telefone;
    medicoExistente.Email = medicoDto.Email;
    medicoExistente.Funcao = medicoDto.Funcao;

    await _medicoRepository.UpdateAsync(medicoExistente);

    _cache.Remove(CacheKey);

    return new ResultWrapper<bool>
    {
        Success = true,
        Message = "Médico atualizado com sucesso.",
        Data = true,
        Errors = new List<string>()
    };
}
}
