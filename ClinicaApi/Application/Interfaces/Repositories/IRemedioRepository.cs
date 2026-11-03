namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Domain.Entities;

public interface IRemedioRepository
{
    Task<IEnumerable<Remedio>> GetAllAsync();

    Task<Remedio?> GetByIdAsync(int id);

    Task<Remedio?> GetByNameAsync(string nome);
    
    Task AddAsync(Remedio remedio);
    Task UpdateAsync(Remedio remedio);
    Task DeleteAsync(int id);
}