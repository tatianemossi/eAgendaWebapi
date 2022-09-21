﻿using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloTarefa;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ServicoTarefa servicoTarefa;
        private readonly IMapper mapeadorTarefas;

        public TarefasController(ServicoTarefa servicoTarefa, IMapper mapeadorTarefas)
        {
            this.servicoTarefa = servicoTarefa;
            this.mapeadorTarefas = mapeadorTarefas;
        }

        [HttpGet]
        public List<ListarTarefasViewModel> SelecionarTodos()
        {
            var tarefaResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos);

            if (tarefaResult.IsSuccess)
                return mapeadorTarefas.Map<List<ListarTarefasViewModel>>(tarefaResult.Value);

            return null;
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public VisualizarTarefaViewModel SelecionarTarefaCompletaPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsSuccess)
                return mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value);

            return null;
        }

        [HttpPost]
        public FormsTarefasViewModel Inserir(InserirTarefaViewModel tarefaVM) 
        {
            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);

            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsSuccess)
                return tarefaVM;

            return null;
        }

        [HttpPut("{id:guid}")]
        public FormsTarefasViewModel Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {
            var tarefaSelecionada = servicoTarefa.SelecionarPorId(id).Value;

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaSelecionada);

            var tarefaResult = servicoTarefa.Editar(tarefa);

            if (tarefaResult.IsSuccess)
                return tarefaVM;

            return null;
        }

        [HttpDelete("{id:guid}")]
        public void Excluir(Guid id)
        {
            servicoTarefa.Excluir(id);
        }
    }
}
