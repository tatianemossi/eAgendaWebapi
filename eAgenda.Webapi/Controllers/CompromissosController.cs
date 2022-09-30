using AutoMapper;
using eAgenda.Aplicacao.ModuloCompromisso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Webapi.ViewModels.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CompromissosController : eAgendaControllerBase
    {
        private readonly ServicoCompromisso servicoCompromisso;
        private readonly IMapper mapeadorCompromissos;

        public CompromissosController(ServicoCompromisso servicoCompromisso, IMapper mapeadorCompromissos)
        {
            this.servicoCompromisso = servicoCompromisso;
            this.mapeadorCompromissos = mapeadorCompromissos;
        }


        [HttpGet]
        public ActionResult<List<ListarCompromissosViewModel>> SelecionarTodos()
        {
            var compromissoResult = servicoCompromisso.SelecionarTodosPeloUsuarioId(UsuarioLogado.Id);
            
            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<List<ListarCompromissosViewModel>>(compromissoResult.Value)
            });
        }

        [HttpGet, Route("entre/{dataInicial:datetime}/{dataFinal:datetime}")]
        public ActionResult<List<ListarCompromissosViewModel>> SelecionarCompromissosFuturos(DateTime dataInicial, DateTime dataFinal)
        {
            var compromissoResult = servicoCompromisso.SelecionarCompromissosFuturos(dataInicial, dataFinal);

            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<List<ListarCompromissosViewModel>>(compromissoResult.Value)
            });
        }

        [HttpGet, Route("passados/{dataAtual:datetime}")]
        public ActionResult<List<ListarCompromissosViewModel>> SelecionarCompromissosPassados(DateTime dataAtual)
        {
            var compromissoResult = servicoCompromisso.SelecionarCompromissosPassados(dataAtual);

            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<List<ListarCompromissosViewModel>>(compromissoResult.Value)
            });
        }

        [HttpGet("vizualizacao-completa/{id:guid}")]
        public ActionResult<VisualizarCompromissoViewModel> SelecionarCompromissoCompletoPorId(Guid id)
        {
            var compromissoResult = servicoCompromisso.SelecionarPorId(id);

            if (compromissoResult.IsFailed && RegistroNaoEncontrado(compromissoResult))
                return NotFound(compromissoResult);

            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<VisualizarCompromissoViewModel>(compromissoResult.Value)
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<FormsCompromissosViewModel> SelecionarCompromissoPorId(Guid id)
        {
            var compromissoResult = servicoCompromisso.SelecionarPorId(id);

            if (compromissoResult.IsFailed && RegistroNaoEncontrado(compromissoResult))
                return NotFound(compromissoResult);

            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<FormsCompromissosViewModel>(compromissoResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<FormsCompromissosViewModel> Inserir(FormsCompromissosViewModel compromissoVM)
        {
            var compromisso = mapeadorCompromissos.Map<Compromisso>(compromissoVM);

            compromisso.UsuarioId = UsuarioLogado.Id;

            var compromissoResult = servicoCompromisso.Inserir(compromisso);

            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = compromissoVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsCompromissosViewModel> Editar(Guid id, FormsCompromissosViewModel compromissoVM)
        {
            var compromissoResult = servicoCompromisso.SelecionarPorId(id);

            if (compromissoResult.IsFailed && RegistroNaoEncontrado(compromissoResult))
                return NotFound(compromissoResult);

            var compromisso = mapeadorCompromissos.Map(compromissoVM, compromissoResult.Value);

            compromissoResult = servicoCompromisso.Editar(compromisso);

            if (compromissoResult.IsFailed)
                return InternalError(compromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = compromissoVM
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var compromissoResult = servicoCompromisso.Excluir(id);

            if (compromissoResult.IsFailed && RegistroNaoEncontrado<Compromisso>(compromissoResult))
                return NotFound(compromissoResult);

            if (compromissoResult.IsFailed)
                return InternalError<Compromisso>(compromissoResult);

            return NoContent();
        }
    }
}
