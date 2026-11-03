namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;

public interface IConsultaService
{
    Task<ResultWrapper<IEnumerable<Consulta>>> GetAllAsync();
    Task<ResultWrapper<Consulta>> GetByIdAsync(int id);
    Task<ResultWrapper<IEnumerable<Consulta>>> GetByMedicoAsync(int idMedico);
    Task<ResultWrapper<IEnumerable<Consulta>>> GetByPacienteAsync(int idPaciente);
    Task<ResultWrapper<bool>> AddAsync(Consulta consulta);
    Task<ResultWrapper<bool>> UpdateAsync(Consulta consulta);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}