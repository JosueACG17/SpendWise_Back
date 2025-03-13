using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Threading.Tasks;

namespace SpendWise.Repositories
{
    public class PerfilRepository
    {
        private readonly AppDbContext _context;

        public PerfilRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Perfil> CreatePerfilAsync(Perfil perfil)
        {
            _context.Perfiles.Add(perfil);
            await _context.SaveChangesAsync();
            return perfil;
        }

        public async Task<Perfil> GetPerfilByIdAsync(int id)
        {
            return await _context.Perfiles.FindAsync(id);
        }
    }
}