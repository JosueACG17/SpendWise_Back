using SpendWise.Models;
using SpendWise.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public class UsuariosService
    {
        private readonly UsuariosRepository _repository;

        public UsuariosService(UsuariosRepository repository)
        {
            _repository = repository;
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            return await _repository.GetUsuarioByEmailAsync(email);
        }

        public async Task AddUsuarioAsync(Usuario usuario)
        {
            await _repository.AddAsync(usuario);
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            await _repository.UpdateAsync(usuario);
        }

        public async Task DeleteUsuarioAsync(int usuarioId)
        {
            await _repository.DeleteUsuarioAsync(usuarioId);
        }

        public async Task<List<Usuario>> GetAllUsuariosAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}