using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Webapi.ViewModels.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DespesasController : eAgendaControllerBase
    {
        private readonly ServicoDespesa servicoDespesa;
        private readonly IMapper mapeadorDespesa;

        public DespesasController(ServicoDespesa servicoDespesa, IMapper mapeadorDespesa)
        {
            this.servicoDespesa = servicoDespesa;
            this.mapeadorDespesa = mapeadorDespesa;
        }

        [HttpGet]
        public ActionResult<List<ListarDespesasViewModel>> SelecionarTodos()
        {
            var despesaResult = servicoDespesa.SelecionarTodosPeloUsuarioId(UsuarioLogado.Id);

            if (despesaResult.IsFailed)
                return InternalError(despesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesa.Map<List<ListarDespesasViewModel>>(despesaResult.Value)
            });
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarDespesaViewModel> SelecionarDespesaCompletaPorId(Guid id)
        {
            var despesaResult = servicoDespesa.SelecionarPorId(id);

            if (despesaResult.IsFailed && RegistroNaoEncontrado(despesaResult))
                return NotFound(despesaResult);

            if (despesaResult.IsFailed)
                return InternalError(despesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesa.Map<VisualizarDespesaViewModel>(despesaResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<FormsDespesaViewModel> Inserir(FormsDespesaViewModel despesaVM)
        {
            var despesa = mapeadorDespesa.Map<Despesa>(despesaVM);

            despesa.UsuarioId = UsuarioLogado.Id;

            var despesaResult = servicoDespesa.Inserir(despesa);

            if (despesaResult.IsFailed)
                return InternalError(despesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = despesaVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsDespesaViewModel> Editar(Guid id, FormsDespesaViewModel despesaVM)
        {
            var despesaResult = servicoDespesa.SelecionarPorId(id);
            despesaResult.Value.Categorias.Clear();

            if (despesaResult.IsFailed && RegistroNaoEncontrado(despesaResult))
                return NotFound(despesaResult);

            var despesa = mapeadorDespesa.Map(despesaVM, despesaResult.Value);

            despesaResult = servicoDespesa.Editar(despesa);

            if (despesaResult.IsFailed)
                return InternalError(despesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = despesaVM
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var despesaResult = servicoDespesa.Excluir(id);

            if (despesaResult.IsFailed && RegistroNaoEncontrado<Despesa>(despesaResult))
                return NotFound(despesaResult);

            if (despesaResult.IsFailed)
                return InternalError<Despesa>(despesaResult);

            return NoContent();
        }
    }
}
