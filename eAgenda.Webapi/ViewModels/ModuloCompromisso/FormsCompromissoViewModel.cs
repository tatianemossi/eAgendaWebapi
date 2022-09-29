using AutoMapper.Configuration.Annotations;
using eAgenda.Dominio.ModuloCompromisso;
using System;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.Webapi.ViewModels.ModuloCompromisso
{
    public class FormsCompromissosViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Assunto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(TipoLocalCompromissoEnum), 
            ErrorMessage = "O número inserido não faz parte do Enum de Tipo de Local, digite um número entre 0 e 1" )]
        public TipoLocalCompromissoEnum TipoLocal { get; set; }
        
        public string Local { get; set; }

        public string Link { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraTermino { get; set; }

        public Guid ContatoId { get; set; }
    }
}
