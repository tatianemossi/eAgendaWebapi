using eAgenda.Dominio.ModuloTarefa;
using System.Collections.Generic;

namespace eAgenda.Webapi.ViewModels.ModuloTarefa
{
    public class FormsTarefasViewModel
    {
        public string Titulo { get; set; }

        public PrioridadeTarefaEnum Prioridade { get; set; }

        public List<FormsItemTarefaViewModel> Itens { get; set; }
    }
}
