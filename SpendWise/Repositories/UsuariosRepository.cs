using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Repositories
{
    public class UsuariosRepository
    {
        private readonly AppDbContext _context;

        public UsuariosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // Incluir el rol
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // Incluir el rol
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
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

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // Incluir el rol
                .ToListAsync();
        }
    }
}