using ClinicaApi.Application.Interfaces.Repositories;
using ClinicaApi.Domain.Entities;
using ClinicaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaApi.Infrastructure.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly AppDbContext _context;

    public LoginRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Login?> GetByCpfAsync(string cpf)
    {
        return await _context.Logins.FirstOrDefaultAsync(l => l.Cpf == cpf);
    }

    public async Task AddAsync(Login login)
    {
        await _context.Logins.AddAsync(login);
        await _context.SaveChangesAsync();
    }
}