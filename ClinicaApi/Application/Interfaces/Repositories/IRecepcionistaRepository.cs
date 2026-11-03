namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Application.DTOs;
using ClinicaApi.Domain.Entities;

public interface IRecepcionistaRepository
{
    Task<IEnumerable<Recepcionista>> GetAllAsync();

    Task<Recepcionista?> GetByIdAsync(int id);

    Task<Recepcionista?> GetByCpfAsync(string cpf);
    
    Task AddAsync(Recepcionista recepcionista);
    Task UpdateAsync(Recepcionista recepcionista);
    Task DeleteAsync(int id);
}