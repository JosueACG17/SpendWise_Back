using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Services
{
    public interface IGastosService
    {
        Task<IEnumerable<Gasto>> GetAllGastosAsync();
        Task<Gasto> GetGastosByIdAsync(int id);
        Task<Gasto> CreateGastosAsync(GastoDTO gastoDTO, string folderName);
        Task UpdateGastosAsync(int id, GastoDTO gastoDTO, string folderName);
        Task DeleteGastosAsync(int id);
    }

    public class GastosService : IGastosService
    {
        private readonly IGastosRepository _gastosRepository;
        private readonly CloudinaryService _cloudinaryService;

        public GastosService(IGastosRepository gastosRepository, CloudinaryService cloudinaryService)
        {
            _gastosRepository = gastosRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<Gasto>> GetAllGastosAsync()
        {
            return await _gastosRepository.GetAllGastosAsync();
        }

        public async Task<Gasto> GetGastosByIdAsync(int id)
        {
            return await _gastosRepository.GetGastoByIdAsync(id);
        }

        public async Task<Gasto> CreateGastosAsync(GastoDTO gastoDTO, string folderName)
        {
            var gasto = new Gasto
            {
                UsuarioId = gastoDTO.UsuarioId,
                //NombreCompleto = gastoDTO.NombreCompleto,
                //Telefono = gastoDTO.Telefono,
                //FechaNacimiento = gastoDTO.FechaNacimiento,
                //Genero = gastoDTO.Genero
            };

            return await _gastosRepository.CreateGastoAsync(gasto);
        }

        public async Task UpdateGastosAsync(int id, GastoDTO gastoDTO, string folderName)
        {
            var gasto = await _gastosRepository.GetGastoByIdAsync(id);
            if (gasto != null)
            {
                gasto.UsuarioId = gastoDTO.UsuarioId;
                //perfil.NombreCompleto = gastoDTO.NombreCompleto;
                //perfil.Telefono = gastoDTO.Telefono;
                //perfil.FechaNacimiento = gastoDTO.FechaNacimiento;
                //perfil.Genero = gastoDTO.Genero;

                await _gastosRepository.UpdateGastoAsync(gasto);
            }
        }

        public async Task DeleteGastosAsync(int id)
        {
            await _gastosRepository.DeleteGastoAsync(id);
        }
    }
}