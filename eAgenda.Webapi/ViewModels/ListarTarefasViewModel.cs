﻿using System;

namespace eAgenda.Webapi.ViewModels
{
    public class ListarTarefasViewModel
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }

        public string Prioridade { get; set; }

        public string Situacao { get; set; }
    }
}
