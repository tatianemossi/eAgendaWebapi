using eAgenda.Dominio.ModuloTarefa;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.Webapi.ViewModels.ModuloTarefa
{
    public class FormsTarefaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(PrioridadeTarefaEnum), 
            ErrorMessage = "O número inserido não faz parte do Enum de Prioridades, digite um número entre 0 e 2")]
        public PrioridadeTarefaEnum Prioridade { get; set; }

        public List<FormsItemTarefaViewModel> Itens { get; set; }
    }

    public class InserirTarefaViewModel : FormsTarefaViewModel {}

    public class EditarTarefaViewModel : FormsTarefaViewModel {}
}
