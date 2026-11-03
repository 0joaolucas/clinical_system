using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaApi.Infrastructure.Repositories;

public class MedicoRepository : IMedicoRepository
{

    private readonly AppDbContext _context;

    public MedicoRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Medico medico)
    {
        await _context.Medicos.AddAsync(medico);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var medico = await GetByIdAsync(id);
        if (medico is null)
        {
            return;
        }

        _context.Medicos.Remove(medico);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Medico>> GetAllAsync()
    {
        return await _context.Medicos.ToListAsync();
    }

    public async Task<Medico?> GetByCpfAsync(string cpf)
    {
        return await _context.Medicos.FirstOrDefaultAsync(m => m.Cpf == cpf);
    }

    public async Task<Medico?> GetByCrmAsync(string crm)
    {
        return await _context.Medicos.FirstOrDefaultAsync(m => m.Crm == crm);
    }

    public async Task<Medico?> GetByIdAsync(int id)
    {
        return await _context.Medicos.FirstOrDefaultAsync(m => m.IdMedico == id);
    }

    public async Task UpdateAsync(Medico medico)
    {
        _context.Medicos.Update(medico);
        await _context.SaveChangesAsync();
    }
}