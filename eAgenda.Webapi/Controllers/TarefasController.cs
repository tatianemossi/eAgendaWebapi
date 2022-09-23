using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloTarefa;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : eAgendaControllerBase
    {
        private readonly ServicoTarefa servicoTarefa;
        private readonly IMapper mapeadorTarefas;

        public TarefasController(ServicoTarefa servicoTarefa, IMapper mapeadorTarefas)
        {
            this.servicoTarefa = servicoTarefa;
            this.mapeadorTarefas = mapeadorTarefas;
        }

        [HttpGet]
        public ActionResult<List<ListarTarefasViewModel>> SelecionarTodos()
        {
            var tarefaResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return RetornarOkComMap<List<Tarefa>, List<ListarTarefasViewModel>>(tarefaResult);
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarTarefaViewModel> SelecionarTarefaCompletaPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsFailed && RegistroNaoEncontrado(tarefaResult))
                return NotFound(tarefaResult);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return RetornarOkComMap<Tarefa, VisualizarTarefaViewModel>(tarefaResult);
        }

        [HttpPost]
        public ActionResult<FormsTarefasViewModel> Inserir(InserirTarefaViewModel tarefaVM)
        {
            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);

            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return RetornarOkSemMap<InserirTarefaViewModel>(tarefaVM);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsTarefasViewModel> Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {

            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsFailed && RegistroNaoEncontrado(tarefaResult))
                return NotFound(tarefaResult);

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaResult.Value);

            tarefaResult = servicoTarefa.Editar(tarefa);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return RetornarOkSemMap<EditarTarefaViewModel>(tarefaVM);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var tarefaResult = servicoTarefa.Excluir(id);

            if (tarefaResult.IsFailed && RegistroNaoEncontrado<Tarefa>(tarefaResult))
                return NotFound(tarefaResult);

            if (tarefaResult.IsFailed)
                return InternalError<Tarefa>(tarefaResult);

            return NoContent();
        }


        #region Métodos Privados
        private ActionResult RetornarOkComMap<TInput, TOutput>(Result<TInput> tarefaResult)
        {
            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<TOutput>(tarefaResult.Value)
            });
        }

        private ActionResult RetornarOkSemMap<T>(Result<T> tarefaResult)
        {
            return Ok(new
            {
                sucesso = true,
                dados = tarefaResult
            });
        }

        #endregion
    }
}