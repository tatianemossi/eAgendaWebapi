using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloTarefa;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    erros = tarefaResult.Errors.Select(x => x.Message)
                });
            }

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<List<ListarTarefasViewModel>>(tarefaResult.Value)
            });
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarTarefaViewModel> SelecionarTarefaCompletaPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
            {
                return NotFound(new
                {
                    sucesso = false,
                    erros = tarefaResult.Errors.Select(x => x.Message)
                });
            }

            if (tarefaResult.IsFailed)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    erros = tarefaResult.Errors.Select(x => x.Message)
                });
            }

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value)
            });
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
