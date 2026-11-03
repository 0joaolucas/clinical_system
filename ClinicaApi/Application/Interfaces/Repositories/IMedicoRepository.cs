namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Domain.Entities;

public interface IMedicoRepository
{
    Task<IEnumerable<Medico>> GetAllAsync();

    Task<Medico?> GetByIdAsync(int id);

    Task<Medico?> GetByCpfAsync(string cpf);

    Task<Medico?> GetByCrmAsync(string crm);
    
    Task AddAsync(Medico medico);
    Task UpdateAsync(Medico medico);
    Task DeleteAsync(int id);
}