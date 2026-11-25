
using ClinicaApi.Application.Common;
using ClinicaApi.Application.Interfaces.Services;
using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicaApi.Application.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "ListaPacientes";

    public PacienteService(
        IPacienteRepository pacienteRepository,
        IMemoryCache cache)
    {
        _pacienteRepository = pacienteRepository;
        _cache = cache;
    }

    public async Task<ResultWrapper<IEnumerable<Paciente>>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<Paciente>? pacientes)
            || pacientes == null)
        {
            pacientes = await _pacienteRepository.GetAllAsync();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheKey, pacientes, options);
        }

        return new ResultWrapper<IEnumerable<Paciente>>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = pacientes,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<Paciente>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return new ResultWrapper<Paciente>
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

        var paciente = await _pacienteRepository.GetByIdAsync(id);

        if (paciente == null)
        {
            return new ResultWrapper<Paciente>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "Paciente não encontrado."
                }
            };
        }

        return new ResultWrapper<Paciente>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = paciente,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<Paciente>> GetByCpfAsync(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return new ResultWrapper<Paciente>
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

        var paciente = await _pacienteRepository.GetByCpfAsync(cpf);

        if (paciente == null)
        {
            return new ResultWrapper<Paciente>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = null,
                Errors = new List<string>
                {
                    "Paciente não encontrado."
                }
            };
        }

        return new ResultWrapper<Paciente>
        {
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = paciente,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<bool>> AddAsync(Paciente paciente)
    {
        if (paciente == null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Paciente é obrigatório."
                }
            };
        }

        if (string.IsNullOrWhiteSpace(paciente.Nome))
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Nome do paciente é obrigatório."
                }
            };
        }

        if (string.IsNullOrWhiteSpace(paciente.Cpf))
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

        var pacienteExistente =
            await _pacienteRepository.GetByCpfAsync(paciente.Cpf);

        if (pacienteExistente != null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Já existe um paciente cadastrado com este CPF."
                }
            };
        }

        await _pacienteRepository.AddAsync(paciente);

        _cache.Remove(CacheKey);

        return new ResultWrapper<bool>
        {
            Success = true,
            Message = "Paciente cadastrado com sucesso.",
            Data = true,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<bool>> UpdateAsync(Paciente paciente)
    {
        if (paciente == null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Paciente é obrigatório."
                }
            };
        }

        if (paciente.IdPaciente <= 0)
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

        if (string.IsNullOrWhiteSpace(paciente.Nome))
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Nome do paciente é obrigatório."
                }
            };
        }

        if (string.IsNullOrWhiteSpace(paciente.Cpf))
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

        var pacienteExistente =
            await _pacienteRepository.GetByIdAsync(paciente.IdPaciente);

        if (pacienteExistente == null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Paciente não encontrado."
                }
            };
        }

        var pacientePorCpf =
            await _pacienteRepository.GetByCpfAsync(paciente.Cpf);

        if (pacientePorCpf != null &&
            pacientePorCpf.IdPaciente != paciente.IdPaciente)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Já existe outro paciente cadastrado com este CPF."
                }
            };
        }

        pacienteExistente.Nome = paciente.Nome;
        pacienteExistente.Cpf = paciente.Cpf;
        pacienteExistente.DataNasc = paciente.DataNasc;
        pacienteExistente.Sexo = paciente.Sexo;
        pacienteExistente.Telefone = paciente.Telefone;
        pacienteExistente.Email = paciente.Email;
        pacienteExistente.Cidade = paciente.Cidade;
        pacienteExistente.Bairro = paciente.Bairro;
        pacienteExistente.Rua = paciente.Rua;
        pacienteExistente.Historico = paciente.Historico;

        await _pacienteRepository.UpdateAsync(pacienteExistente);

        _cache.Remove(CacheKey);

        return new ResultWrapper<bool>
        {
            Success = true,
            Message = "Paciente atualizado com sucesso.",
            Data = true,
            Errors = new List<string>()
        };
    }

    public async Task<ResultWrapper<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
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

        var paciente = await _pacienteRepository.GetByIdAsync(id);

        if (paciente == null)
        {
            return new ResultWrapper<bool>
            {
                Success = false,
                Message = "Não foi possível realizar a operação.",
                Data = false,
                Errors = new List<string>
                {
                    "Paciente não encontrado."
                }
            };
        }

        await _pacienteRepository.DeleteAsync(id);

        _cache.Remove(CacheKey);

        return new ResultWrapper<bool>
        {
            Success = true,
            Message = "Paciente excluído com sucesso.",
            Data = true,
            Errors = new List<string>()
        };
    }
}
