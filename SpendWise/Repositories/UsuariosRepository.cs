using Microsoft.EntityFrameworkCore;
using SpendWise.Models;

public class UsuariosRepository
{
    private readonly AppDbContext _context;

    public UsuariosRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> GetUsuarioByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email); 
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsuarioAsync(int usuarioId)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);

        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

}