using AutoMapper;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.ModuloCategoria;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<FormsCategoriaViewModel, Categoria>();

            CreateMap<Categoria, ListarCategoriaViewModel>();
            CreateMap<Categoria, VisualizarCategoriaViewModel>();
        }
    }
}
