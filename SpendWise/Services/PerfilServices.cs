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
        Task<Perfil> CreatePerfilAsync(PerfilDTO perfilDTO, string folderName);
        Task UpdatePerfilAsync(int id, PerfilDTO perfilDTO, string folderName);
        Task DeletePerfilAsync(int id);
    }

    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;
        private readonly CloudinaryService _cloudinaryService;

        public PerfilService(IPerfilRepository perfilRepository, CloudinaryService cloudinaryService)
        {
            _perfilRepository = perfilRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<Perfil>> GetAllPerfilesAsync()
        {
            return await _perfilRepository.GetAllPerfilesAsync();
        }

        public async Task<Perfil> GetPerfilByIdAsync(int id)
        {
            return await _perfilRepository.GetPerfilByIdAsync(id);
        }

        public async Task<Perfil> CreatePerfilAsync(PerfilDTO perfilDTO, string folderName)
        {
            var perfil = new Perfil
            {
                UsuarioId = perfilDTO.UsuarioId,
                NombreCompleto = perfilDTO.NombreCompleto,
                Telefono = perfilDTO.Telefono,
                FechaNacimiento = perfilDTO.FechaNacimiento,
                Genero = perfilDTO.Genero
            };

            if (perfilDTO.Foto != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageToCloudinary(perfilDTO.Foto, folderName);
                perfil.FotoUrl = uploadResult?.SecureUrl.ToString() ?? "/images/default.jpg";
            }

            return await _perfilRepository.CreatePerfilAsync(perfil);
        }

        public async Task UpdatePerfilAsync(int id, PerfilDTO perfilDTO, string folderName)
        {
            var perfil = await _perfilRepository.GetPerfilByIdAsync(id);
            if (perfil != null)
            {
                perfil.UsuarioId = perfilDTO.UsuarioId;
                perfil.NombreCompleto = perfilDTO.NombreCompleto;
                perfil.Telefono = perfilDTO.Telefono;
                perfil.FechaNacimiento = perfilDTO.FechaNacimiento;
                perfil.Genero = perfilDTO.Genero;

                if (perfilDTO.Foto != null)
                {
                    var uploadResult = await _cloudinaryService.UploadImageToCloudinary(perfilDTO.Foto, folderName);
                    perfil.FotoUrl = uploadResult?.SecureUrl.ToString() ?? "/images/default.jpg";
                }

                await _perfilRepository.UpdatePerfilAsync(perfil);
            }
        }

        public async Task DeletePerfilAsync(int id)
        {
            await _perfilRepository.DeletePerfilAsync(id);
        }
    }
}