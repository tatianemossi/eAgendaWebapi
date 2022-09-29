﻿using eAgenda.Webapi.ViewModels.ModuloCategoria;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.ViewModels.ModuloDespesa
{
    public class ListarDespesasViewModel
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; }

        public List<VisualizarCategoriaViewModel> Categorias { get; set; }
    }
}
