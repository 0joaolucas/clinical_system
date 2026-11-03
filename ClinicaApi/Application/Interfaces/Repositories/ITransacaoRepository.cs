namespace ClinicaApi.Application.Interfaces.Repositories;

using ClinicaApi.Domain.Entities;

public interface ITransacaoRepository
{
    Task<IEnumerable<Transacao>> GetAllAsync();

    Task<Transacao?> GetByIdAsync(int id);

    Task<IEnumerable<Transacao>> GetByRemedioAsync(int remedioId);

    Task<IEnumerable<Transacao>> GetByPacienteAsync(int pacienteId);

    Task AddAsync(Transacao transacao);

    Task UpdateAsync(Transacao transacao);

    Task DeleteAsync(int id);
}