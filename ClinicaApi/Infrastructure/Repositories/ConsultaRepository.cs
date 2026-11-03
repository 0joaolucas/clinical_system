using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaApi.Infrastructure.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly AppDbContext _context;

    public ConsultaRepository (AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Consulta consulta)
    {
        await _context.Consultas.AddAsync(consulta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var consulta = await GetByIdAsync(id);
        if (consulta is null)
        {
            return;
        }

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Consulta>> GetAllAsync()
    {
        return await _context.Consultas.ToListAsync();
    }

    public async Task<Consulta?> GetByIdAsync(int id)
    {
        return await _context.Consultas.FirstOrDefaultAsync(c => c.IdConsulta == id);
    }

    public async Task<IEnumerable<Consulta>> GetByMedicoAsync(int medicoId)
    {
        return await _context.Consultas.Where(c => c.IdMedico == medicoId).ToListAsync();
    }

    public async Task<IEnumerable<Consulta>> GetByPacienteAsync(int pacienteId)
    {
        return await _context.Consultas.Where(c => c.IdPaciente == pacienteId).ToListAsync();
    }

    public async Task UpdateAsync(Consulta consulta)
    {
        _context.Consultas.Update(consulta);
        await _context.SaveChangesAsync();
    }
}