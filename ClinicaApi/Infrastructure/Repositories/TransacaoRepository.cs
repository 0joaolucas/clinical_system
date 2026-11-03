using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaApi.Infrastructure.Repositories;

public class TransacaoRepository : ITransacaoRepository
{

    private readonly AppDbContext _context;

    public TransacaoRepository (AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Transacao transacao)
    {
        await _context.Transacoes.AddAsync(transacao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transacao = await GetByIdAsync(id);
        if(transacao is null)
        {
            return;
        }

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transacao>> GetAllAsync()
    {
        return await _context.Transacoes.ToListAsync();
    }

    public async Task<Transacao?> GetByIdAsync(int id)
    {
        return await _context.Transacoes.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Transacao>> GetByPacienteAsync(int pacienteId)
    {
        return await _context.Transacoes.Where(t => t.IdPaciente == pacienteId).ToListAsync();
    }

    public async Task<IEnumerable<Transacao>> GetByRemedioAsync(int remedioId)
    {
        return await _context.Transacoes.Where(t => t.IdRemedio == remedioId).ToListAsync();
    }

    public async Task UpdateAsync(Transacao transacao)
    {
        _context.Transacoes.Update(transacao);
        await _context.SaveChangesAsync();
    }
}