using AutoMapper;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class ContatoProfile : Profile
    {
        public ContatoProfile()
        {
            CreateMap<FormsContatoViewModel, Contato>();

            CreateMap<Contato, ListarContatosViewModel>();
            CreateMap<Contato, VisualizarContatoViewModel>();
        }
    }
}
