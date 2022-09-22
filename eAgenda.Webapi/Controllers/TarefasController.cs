using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloTarefa;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
        public ActionResult<List<ListarTarefasViewModel>> SelecionarTodos()
        {
            var tarefaResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos);

            if (tarefaResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, tarefaResult.Errors.Select(x => x.Message));

            return RetornarOkComMap<List<Tarefa>, List<ListarTarefasViewModel>>(tarefaResult);
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarTarefaViewModel> SelecionarTarefaCompletaPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
                return RetornaErro(HttpStatusCode.NotFound, tarefaResult.Errors.Select(x => x.Message));

            if (tarefaResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, tarefaResult.Errors.Select(x => x.Message));

            return RetornarOkComMap<Tarefa, VisualizarTarefaViewModel>(tarefaResult);
        }

        [HttpPost]
        public ActionResult<FormsTarefasViewModel> Inserir(InserirTarefaViewModel tarefaVM)
        {
            var listaErros = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);

            if (listaErros.Any())
                return RetornaErro(HttpStatusCode.BadRequest, listaErros);

            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);

            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, tarefaResult.Errors.Select(x => x.Message));

            return RetornarOkSemMap<InserirTarefaViewModel>(tarefaVM);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsTarefasViewModel> Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {
            var listaErros = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);

            if (listaErros.Any())
                return RetornaErro(HttpStatusCode.BadRequest, listaErros);

            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
                return RetornaErro(HttpStatusCode.NotFound, tarefaResult.Errors.Select(x => x.Message));

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaResult.Value);

            tarefaResult = servicoTarefa.Editar(tarefa);

            if (tarefaResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, tarefaResult.Errors.Select(x => x.Message));

            return RetornarOkSemMap<EditarTarefaViewModel>(tarefaVM);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var tarefaResult = servicoTarefa.Excluir(id);

            if (tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
                return RetornaErro(HttpStatusCode.NotFound, tarefaResult.Errors.Select(x => x.Message));

            if (tarefaResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, tarefaResult.Errors.Select(x => x.Message));

            return NoContent();
        }

        private ActionResult RetornaErro(HttpStatusCode statusCode, IEnumerable<string> erros)
        {
            return StatusCode((int)statusCode, new
            {
                sucesso = false,
                erros = erros
            });
        }

        public ActionResult RetornarOkComMap<TInput, TOutput>(Result<TInput> tarefaResult)
        {
            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<TOutput>(tarefaResult.Value)
            });
        }

        public ActionResult RetornarOkSemMap<T>(Result<T> tarefaResult)
        {
            return Ok(new
            {
                sucesso = true,
                dados = tarefaResult
            });
        }
    }
}