using System.ComponentModel.DataAnnotations;

namespace eAgenda.Webapi.ViewModels.ModuloCategoria
{
    public class FormsCategoriaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Titulo { get; set; }
    }
}
