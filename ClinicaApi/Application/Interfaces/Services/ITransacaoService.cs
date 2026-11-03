namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;

public interface ITransacaoService
{
    Task<ResultWrapper<IEnumerable<Transacao>>> GetAllAsync();
    Task<ResultWrapper<Transacao>> GetByIdAsync(int id);
    Task<ResultWrapper<IEnumerable<Transacao>>> GetByPacienteAsync(int idPaciente);
    Task<ResultWrapper<IEnumerable<Transacao>>> GetByRemedioAsync(int idRemedio);
    Task<ResultWrapper<bool>> AddAsync(Transacao transacao);
    Task<ResultWrapper<bool>> UpdateAsync(Transacao transacao);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}