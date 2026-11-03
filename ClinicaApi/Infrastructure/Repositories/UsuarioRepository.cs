namespace ClinicaApi.Infrastructure.Repositories;

using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{

    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await GetByIdAsync(id);
        if (usuario is null)
        {
            return;
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetByCpfAsync(string cpf)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Cpf == cpf);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }
}