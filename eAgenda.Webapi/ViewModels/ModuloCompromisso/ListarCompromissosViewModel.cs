using eAgenda.Webapi.ViewModels.ModuloContato;
using System;

namespace eAgenda.Webapi.ViewModels.ModuloCompromisso
{
    public class ListarCompromissosViewModel
    {
        public Guid Id { get; set; }

        public string Assunto { get; set; }

        public string TipoLocal { get; set; }

        public VisualizarContatoViewModel Contato { get; set; }
    }
}
