using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaApi.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly AppDbContext _context;

    public PacienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllAsync()
    {
        return await _context.Pacientes.ToListAsync();
    }

    public async Task<Paciente?> GetByIdAsync(int id)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(p => p.IdPaciente == id);
    }

    public async Task<Paciente?> GetByCpfAsync(string cpf)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Cpf == cpf);
    }

    public async Task AddAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Paciente paciente)
    {
        _context.Pacientes.Update(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var paciente = await GetByIdAsync(id);

        if (paciente != null)
        {
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
        }
    }
}