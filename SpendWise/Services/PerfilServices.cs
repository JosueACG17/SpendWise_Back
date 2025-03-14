using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public interface IPerfilService
    {
        Task<IEnumerable<Perfil>> GetAllPerfilesAsync();
        Task<Perfil> GetPerfilByIdAsync(int id);
        Task<Perfil> CreatePerfilAsync(PerfilDTO perfilDTO);
        Task UpdatePerfilAsync(int id, PerfilDTO perfilDTO);
        Task DeletePerfilAsync(int id);
    }

    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;

        public PerfilService(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }

        public async Task<IEnumerable<Perfil>> GetAllPerfilesAsync()
        {
            return await _perfilRepository.GetAllPerfilesAsync();
        }

        public async Task<Perfil> GetPerfilByIdAsync(int id)
        {
            return await _perfilRepository.GetPerfilByIdAsync(id);
        }

        public async Task<Perfil> CreatePerfilAsync(PerfilDTO perfilDTO)
        {
            var perfil = new Perfil
            {
                UsuarioId = perfilDTO.UsuarioId,
                NombreCompleto = perfilDTO.NombreCompleto,
                Telefono = perfilDTO.Telefono,
                FechaNacimiento = perfilDTO.FechaNacimiento,
                Genero = perfilDTO.Genero
            };

            return await _perfilRepository.CreatePerfilAsync(perfil);
        }

        public async Task UpdatePerfilAsync(int id, PerfilDTO perfilDTO)
        {
            var perfil = await _perfilRepository.GetPerfilByIdAsync(id);
            if (perfil != null)
            {
                perfil.UsuarioId = perfilDTO.UsuarioId;
                perfil.NombreCompleto = perfilDTO.NombreCompleto;
                perfil.Telefono = perfilDTO.Telefono;
                perfil.FechaNacimiento = perfilDTO.FechaNacimiento;
                perfil.Genero = perfilDTO.Genero;

                await _perfilRepository.UpdatePerfilAsync(perfil);
            }
        }

        public async Task DeletePerfilAsync(int id)
        {
            await _perfilRepository.DeletePerfilAsync(id);
        }
    }
}