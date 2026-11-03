namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Domain.Entities;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();

    Task<Usuario?> GetByIdAsync(int id);

    Task<Usuario?> GetByCpfAsync(string cpf);

    Task<Usuario?> GetByEmailAsync(string email);
    
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task DeleteAsync(int id);
}