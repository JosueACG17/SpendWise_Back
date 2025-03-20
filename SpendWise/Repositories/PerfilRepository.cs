using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpendWise.Repositories
{
    public interface IPerfilRepository
    {
        Task<IEnumerable<Perfil>> GetAllPerfilesAsync();
        Task<Perfil> GetPerfilByIdAsync(int id);
        Task<Perfil> CreatePerfilAsync(Perfil perfil);
        Task UpdatePerfilAsync(Perfil perfil);
        Task DeletePerfilAsync(int id);
        Task<Perfil> GetPerfilByUsuarioIdAsync(int usuarioId);
    }

    public class PerfilRepository : IPerfilRepository
    {
        private readonly AppDbContext _context;

        public PerfilRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Perfil>> GetAllPerfilesAsync()
        {
            return await _context.Perfiles.ToListAsync();
        }

        public async Task<Perfil> GetPerfilByIdAsync(int id)
        {
            return await _context.Perfiles.FindAsync(id);
        }

        public async Task<Perfil> CreatePerfilAsync(Perfil perfil)
        {
            _context.Perfiles.Add(perfil);
            await _context.SaveChangesAsync();
            return perfil;
        }

        public async Task UpdatePerfilAsync(Perfil perfil)
        {
            _context.Entry(perfil).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerfilAsync(int id)
        {
            var perfil = await _context.Perfiles.FindAsync(id);
            if (perfil != null)
            {
                _context.Perfiles.Remove(perfil);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Perfil> GetPerfilByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Perfiles
                                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }
    }
}