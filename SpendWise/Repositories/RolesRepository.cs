using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Repositories
{
    public class RolesRepository
    {
        private readonly AppDbContext _context;

        public RolesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}