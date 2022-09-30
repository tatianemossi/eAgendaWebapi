using AutoMapper;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class ContatoProfile : Profile
    {
        public ContatoProfile()
        {
            CreateMap<FormsContatoViewModel, Contato>()
                .ForMember(destino => destino.UsuarioId, opt => opt.MapFrom<UsuarioResolver>());

            CreateMap<Contato, ListarContatosViewModel>();
            CreateMap<Contato, VisualizarContatoViewModel>();
        }
    }
}
