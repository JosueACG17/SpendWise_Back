using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public class PresupuestoService
    {
        private readonly PresupuestoRepository _repository;
        private readonly IMapper _mapper;

        public PresupuestoService(PresupuestoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PresupuestoDTO>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            var presupuestos = await _repository.GetAllByUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<PresupuestoDTO>>(presupuestos);
        }

        public async Task<PresupuestoDTO> GetByIdAsync(int id)
        {
            var presupuesto = await _repository.GetByIdAsync(id);
            return _mapper.Map<PresupuestoDTO>(presupuesto);
        }

        public async Task AddAsync(PresupuestoDTO presupuestoDto)
        {
            var presupuesto = _mapper.Map<Presupuesto>(presupuestoDto);
            await _repository.AddAsync(presupuesto);
        }

        public async Task UpdateAsync(PresupuestoDTO presupuestoDto)
        {
            var presupuesto = _mapper.Map<Presupuesto>(presupuestoDto);
            await _repository.UpdateAsync(presupuesto);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}