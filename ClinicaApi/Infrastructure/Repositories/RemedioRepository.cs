using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaApi.Infrastructure.Repositories;

public class RemedioRepository : IRemedioRepository
{

    private readonly AppDbContext _context;

    public RemedioRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Remedio remedio)
    {
        await _context.Remedios.AddAsync(remedio);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var remedio = await GetByIdAsync(id);
        if (remedio is null)
        {
            return;
        }

        _context.Remedios.Remove(remedio);
        await _context.SaveChangesAsync();
    }

    public async Task<Remedio?> GetByNameAsync(string nome)
    {
        return await _context.Remedios.FirstOrDefaultAsync(r => r.Nome == nome);
    }

    public async Task<IEnumerable<Remedio>> GetAllAsync()
    {
        return await _context.Remedios.ToListAsync();
    }

    public async Task<Remedio?> GetByIdAsync(int id)
    {
        return await _context.Remedios.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Remedio remedio)
    {
        _context.Remedios.Update(remedio);
        await _context.SaveChangesAsync();
    }


}