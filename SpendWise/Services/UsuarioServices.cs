    using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Threading.Tasks;

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

}