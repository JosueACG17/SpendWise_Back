using SpendWise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SpendWise.Repositories
{
    public class EtiquetaRepository
    {
        private readonly AppDbContext _context;

        public EtiquetaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Etiqueta>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Etiquetas
                                 .Where(e => e.UsuarioId == usuarioId)
                                 .ToListAsync();
        }

        public async Task<Etiqueta> GetByIdAsync(int id)
        {
            return await _context.Etiquetas.FindAsync(id);
        }

        public async Task AddAsync(Etiqueta etiqueta)
        {
            await _context.Etiquetas.AddAsync(etiqueta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Etiqueta etiqueta)
        {
            _context.Etiquetas.Update(etiqueta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var etiqueta = await _context.Etiquetas.FindAsync(id);
            if (etiqueta != null)
            {
                _context.Etiquetas.Remove(etiqueta);
                await _context.SaveChangesAsync();
            }
        }
    }
}