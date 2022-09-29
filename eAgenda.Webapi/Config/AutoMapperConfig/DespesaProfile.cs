using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using eAgenda.Webapi.ViewModels.ModuloDespesa;
using System.Linq;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class DespesaProfile : Profile
    {
        public DespesaProfile()
        {
            CreateMap<FormsDespesaViewModel, Despesa>()
                .AfterMap((viewModel, despesa) =>
                {
                    if (viewModel.CategoriasId == null)
                        return;

                    foreach (var categoriasId in viewModel.CategoriasId)
                    {
                        if (!despesa.Categorias.Select(x => x.Id).Contains(categoriasId))
                            despesa.Categorias.Add(new Categoria { Id = categoriasId });
                    }
                });

            CreateMap<Despesa, ListarDespesasViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()));

            CreateMap<Despesa, VisualizarDespesaViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()));

            CreateMap<Categoria, VisualizarCategoriaViewModel>();
        }
    }
}
