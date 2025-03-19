using SpendWise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SpendWise.Repositories
{
    public class PresupuestoRepository
    {
        private readonly AppDbContext _context;

        public PresupuestoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Presupuesto>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Presupuestos
                                 .Where(p => p.UsuarioId == usuarioId)
                                 .ToListAsync();
        }
        public async Task<Presupuesto> GetByIdAsync(int id)
        {
            return await _context.Presupuestos.FindAsync(id);
        }

        public async Task AddAsync(Presupuesto presupuesto)
        {
            await _context.Presupuestos.AddAsync(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Presupuesto presupuesto)
        {
            _context.Presupuestos.Update(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var presupuesto = await _context.Presupuestos.FindAsync(id);
            if (presupuesto != null)
            {
                _context.Presupuestos.Remove(presupuesto);
                await _context.SaveChangesAsync();
            }
        }
    }
}