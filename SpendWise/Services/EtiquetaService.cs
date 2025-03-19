using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public class EtiquetaService
    {
        private readonly EtiquetaRepository _repository;
        private readonly IMapper _mapper;

        public EtiquetaService(EtiquetaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EtiquetaDTO>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            var etiquetas = await _repository.GetAllByUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<EtiquetaDTO>>(etiquetas);
        }

        public async Task<EtiquetaDTO> GetByIdAsync(int id)
        {
            var etiqueta = await _repository.GetByIdAsync(id);
            return _mapper.Map<EtiquetaDTO>(etiqueta);
        }

        public async Task AddAsync(EtiquetaDTO etiquetaDto)
        {
            var etiqueta = _mapper.Map<Etiqueta>(etiquetaDto);
            await _repository.AddAsync(etiqueta);
        }

        public async Task UpdateAsync(EtiquetaDTO etiquetaDto)
        {
            var etiqueta = _mapper.Map<Etiqueta>(etiquetaDto);
            await _repository.UpdateAsync(etiqueta);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}