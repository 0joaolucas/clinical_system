namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Application.DTOs;

public interface IRecepcionistaService
{
    Task<ResultWrapper<IEnumerable<Recepcionista>>> GetAllAsync();
    Task<ResultWrapper<Recepcionista>> GetByIdAsync(int id);
    Task<ResultWrapper<Recepcionista>> GetByCpfAsync(string cpf);
    Task<ResultWrapper<bool>> AddAsync(RecepcionistaDto recepcionistaDto);
    Task<ResultWrapper<bool>> UpdateAsync(Recepcionista recepcionista);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}
