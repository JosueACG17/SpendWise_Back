using AutoMapper;
using SpendWise.DTOs;
using SpendWise.Models;

namespace SpendWise.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Gasto, GastoDTO>().ReverseMap();
            CreateMap<Presupuesto, PresupuestoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Etiqueta, EtiquetaDTO>().ReverseMap();
        }
    }
}
