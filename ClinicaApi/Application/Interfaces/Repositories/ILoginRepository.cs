using ClinicaApi.Domain.Entities;

namespace ClinicaApi.Application.Interfaces.Repositories;

public interface ILoginRepository
{
    Task<Login?> GetByCpfAsync(string cpf);
    Task AddAsync(Login login);
}