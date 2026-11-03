namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;
using ClinicaApi.Domain.Entities;

public interface IUsuarioService
{
    Task<ResultWrapper<IEnumerable<Usuario>>> GetAllAsync();
    Task<ResultWrapper<Usuario>> GetByIdAsync(int id);
    Task<ResultWrapper<Usuario>> GetByEmailAsync(string email);
    Task<ResultWrapper<bool>> AddAsync(Usuario usuario);
    Task<ResultWrapper<bool>> UpdateAsync(Usuario usuario);
    Task<ResultWrapper<bool>> DeleteAsync(int id);
}