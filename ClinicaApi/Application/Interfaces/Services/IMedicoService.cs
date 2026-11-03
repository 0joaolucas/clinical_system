namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Application.DTOs;
using ClinicaApi.Domain.Entities;

public interface IMedicoService
{
    Task<ResultWrapper<IEnumerable<Medico>>> GetAllAsync();
    Task<ResultWrapper<Medico>> GetByIdAsync(int id);
    Task<ResultWrapper<Medico>> GetByCpfAsync(string cpf);
    Task<ResultWrapper<Medico>> GetByCrmAsync(string crm);
    Task<ResultWrapper<bool>> AddAsync(MedicoDto medicoDto);
    Task<ResultWrapper<bool>> UpdateAsync(MedicoDto medicoDto);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}