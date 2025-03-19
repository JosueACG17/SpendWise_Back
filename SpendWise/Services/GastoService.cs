using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using AutoMapper;

namespace SpendWise.Services
{
    public class GastoService
    {
        private readonly GastoRepository _repository;
        private readonly IMapper _mapper;

        public GastoService(GastoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GastoDTO>> GetAllGastosByUsuarioIdAsync(int usuarioId)
        {
            var gastos = await _repository.GetAllByUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<GastoDTO>>(gastos);
        }

        public async Task<GastoDTO> GetGastoByIdAsync(int id)
        {
            var gasto = await _repository.GetByIdAsync(id);
            return _mapper.Map<GastoDTO>(gasto);
        }

        public async Task AddGastoAsync(GastoDTO gastoDto)
        {
            var gasto = _mapper.Map<Gasto>(gastoDto);
            await _repository.AddAsync(gasto);
        }

        public async Task UpdateGastoAsync(GastoDTO gastoDto)
        {
            var gasto = _mapper.Map<Gasto>(gastoDto);
            await _repository.UpdateAsync(gasto);
        }

        public async Task DeleteGastoAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
