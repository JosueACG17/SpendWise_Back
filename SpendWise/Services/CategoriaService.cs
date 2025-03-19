using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public class CategoriaService
    {
        private readonly CategoriaRepository _repository;
        private readonly IMapper _mapper;

        public CategoriaService(CategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            var categorias = await _repository.GetAllByUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
        }

        public async Task<CategoriaDTO> GetByIdAsync(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public async Task AddAsync(CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            await _repository.AddAsync(categoria);
        }

        public async Task UpdateAsync(CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            await _repository.UpdateAsync(categoria);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}