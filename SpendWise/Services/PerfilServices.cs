using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using System;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public class PerfilService
    {
        private readonly PerfilRepository _perfilRepository;
        private readonly Cloudinary _cloudinary;

        public PerfilService(PerfilRepository perfilRepository, Cloudinary cloudinary)
        {
            _perfilRepository = perfilRepository;
            _cloudinary = cloudinary;
        }

        public async Task<Perfil> CreatePerfilAsync(PerfilDTO perfilDto, IFormFile foto)
        {
            var perfil = new Perfil
            {
                NombreCompleto = perfilDto.NombreCompleto,
                Telefono = perfilDto.Telefono,
                FechaNacimiento = perfilDto.FechaNacimiento,
                Genero = perfilDto.Genero,
                FotoUrl = "/images/default.jpg" // Valor por defecto
            };

            if (foto != null && foto.Length > 0)
            {
                var uploadResult = await UploadImageToCloudinary(foto);
                perfil.FotoUrl = uploadResult?.SecureUrl.ToString() ?? "/images/default.jpg";
            }

            return await _perfilRepository.CreatePerfilAsync(perfil);
        }

        public async Task<Perfil> GetPerfilByIdAsync(int id)
        {
            return await _perfilRepository.GetPerfilByIdAsync(id);
        }

        public async Task<ImageUploadResult?> UploadImageToCloudinary(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "perfiles"
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}