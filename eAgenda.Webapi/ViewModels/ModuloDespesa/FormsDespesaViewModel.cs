using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.Webapi.ViewModels.ModuloDespesa
{
    public class FormsDespesaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(FormaPgtoDespesaEnum), 
            ErrorMessage = "O número inserido não faz parte do Enum de Formas de Pagamento, digite um número entre 0 e 2")]
        public FormaPgtoDespesaEnum FormaPagamento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public List<CategoriaSelecionadaViewModel> CategoriasSelecionadas { get; set; }
    }
}
