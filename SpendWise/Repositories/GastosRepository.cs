using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpendWise.Repositories
{
    public interface IGastosRepository
    {
        Task<IEnumerable<Gasto>> GetAllGastosAsync();
        Task<Gasto> GetGastoByIdAsync(int id);
        Task<Gasto> CreateGastoAsync(Gasto gasto);
        Task UpdateGastoAsync(Gasto gasto);
        Task DeleteGastoAsync(int id);
    }

    public class GastosRepository : IGastosRepository
    {
        private readonly AppDbContext _context;

        public GastosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gasto>> GetAllGastosAsync()
        {
            return await _context.Gastos.ToListAsync();
        }

        public async Task<Gasto> GetGastoByIdAsync(int id)
        {
            return await _context.Gastos.FindAsync(id);
        }

        public async Task<Gasto> CreateGastoAsync(Gasto gasto)
        {
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();
            return gasto;
        }

        public async Task UpdateGastoAsync(Gasto gasto)
        {
            _context.Entry(gasto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGastoAsync(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
            }
        }
    }
}