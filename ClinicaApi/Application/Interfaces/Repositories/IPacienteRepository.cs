namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Domain.Entities;

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> GetAllAsync();

    Task<Paciente?> GetByIdAsync(int id);

    Task<Paciente?> GetByCpfAsync(string cpf);

    Task AddAsync(Paciente paciente);
    Task UpdateAsync(Paciente paciente);
    Task DeleteAsync(int id);
}