namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Domain.Entities;

public interface IConsultaRepository
{
    Task<IEnumerable<Consulta>> GetAllAsync();

    Task<Consulta?> GetByIdAsync(int id);

    Task<IEnumerable<Consulta>> GetByMedicoAsync(int medicoId);

    Task<IEnumerable<Consulta>> GetByPacienteAsync(int pacienteId);

    Task AddAsync(Consulta consulta);

    Task UpdateAsync(Consulta consulta);

    Task DeleteAsync(int id);
}