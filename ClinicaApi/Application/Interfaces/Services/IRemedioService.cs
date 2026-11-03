namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;

public interface IRemedioService
{
    Task<ResultWrapper<IEnumerable<Remedio>>> GetAllAsync();
    Task<ResultWrapper<Remedio>> GetByIdAsync(int id);
    Task<ResultWrapper<Remedio>> GetByNomeAsync(string nome);
    Task<ResultWrapper<bool>> AddAsync(Remedio remedio);
    Task<ResultWrapper<bool>> UpdateAsync(Remedio remedio);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}