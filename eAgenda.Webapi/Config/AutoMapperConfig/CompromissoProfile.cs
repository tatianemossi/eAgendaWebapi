using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloCompromisso;
using eAgenda.Webapi.ViewModels.ModuloContato;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class CompromissoProfile : Profile
    {
        public CompromissoProfile()
        {
            CreateMap<FormsCompromissosViewModel, Compromisso>()
                .ForMember(destino => destino.UsuarioId, opt => opt.MapFrom<UsuarioResolver>());

            CreateMap<Compromisso, ListarCompromissosViewModel>()
                .ForMember(destino => destino.TipoLocal, opt => opt.MapFrom(origem => origem.TipoLocal.GetDescription()))
                .ForMember(d => d.NomeContato, opt => opt.MapFrom(o => o.Contato.Nome));

            CreateMap<Compromisso, VisualizarCompromissoViewModel>()
                .ForMember(destino => destino.TipoLocal, opt => opt.MapFrom(origem => origem.TipoLocal.GetDescription()));

            CreateMap<Contato, VisualizarContatoViewModel>();
        }
    }
}
