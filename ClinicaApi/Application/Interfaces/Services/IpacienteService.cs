namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Domain.Entities;
using ClinicaApi.Application.Common;
public interface IPacienteService
{
    Task<ResultWrapper<IEnumerable<Paciente>>> GetAllAsync();
    Task<ResultWrapper<Paciente>> GetByIdAsync(int id);
    Task<ResultWrapper<Paciente>> GetByCpfAsync(string cpf);
    Task<ResultWrapper<bool>> AddAsync(Paciente paciente);
    Task<ResultWrapper<bool>> UpdateAsync(Paciente paciente);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}
