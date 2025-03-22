using SpendWise.Models;
using SpendWise.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public class RolesService
    {
        private readonly RolesRepository _repository;

        public RolesService(RolesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Rol>> GetAllRolesAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}