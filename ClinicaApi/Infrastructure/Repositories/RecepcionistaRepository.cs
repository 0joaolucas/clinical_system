namespace ClinicaApi.Infrastructure.Repositories;

using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RecepcionistaRepository : IRecepcionistaRepository
{

    private readonly AppDbContext _context;

    public RecepcionistaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Recepcionista recepcionista)
    {
        await _context.Recepcionistas.AddAsync(recepcionista);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var recepcionista = await GetByIdAsync(id);
        if (recepcionista is null)
        {
            return;
        }

        _context.Recepcionistas.Remove(recepcionista);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Recepcionista>> GetAllAsync()
    {
        return await _context.Recepcionistas.ToListAsync();
    }

    public async Task<Recepcionista?> GetByCpfAsync(string cpf)
    {
        return await _context.Recepcionistas.FirstOrDefaultAsync(r => r.Cpf == cpf);
    }

    public async Task<Recepcionista?> GetByIdAsync(int id)
    {
        return await _context.Recepcionistas.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Recepcionista recepcionista){

        var recepcionistaExistente = await _context.Recepcionistas
            .FirstOrDefaultAsync(r => r.Id == recepcionista.Id);

        if (recepcionistaExistente == null)
        {
            return;
        }

        recepcionistaExistente.Nome = recepcionista.Nome;
        recepcionistaExistente.Cpf = recepcionista.Cpf;
        recepcionistaExistente.Telefone = recepcionista.Telefone;
        recepcionistaExistente.Email = recepcionista.Email;

        await _context.SaveChangesAsync();
    }
}