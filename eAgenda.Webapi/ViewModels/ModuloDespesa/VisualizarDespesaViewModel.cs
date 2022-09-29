using eAgenda.Dominio.ModuloDespesa;
using System.Collections.Generic;
using System;
using eAgenda.Webapi.ViewModels.ModuloCategoria;

namespace eAgenda.Webapi.ViewModels.ModuloDespesa
{
    public class VisualizarDespesaViewModel
    {
        public VisualizarDespesaViewModel()
        {
            Categorias = new List<VisualizarCategoriaViewModel>();
        }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; }

        public List<VisualizarCategoriaViewModel> Categorias { get; set; }
    }
}
