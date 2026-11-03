namespace ClinicaApi.Application.Interfaces.Services;

using ClinicaApi.Application.Common;

public interface ILoginService
{
    Task<ResultWrapper<string>> LoginAsync(string cpf, string senha);
}