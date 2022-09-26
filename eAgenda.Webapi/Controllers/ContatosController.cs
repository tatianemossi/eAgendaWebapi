using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;
using AutoMapper;
using FluentResults;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : eAgendaControllerBase
    {
        private readonly ServicoContato servicoContato;
        private readonly IMapper mapeadorContatos;

        public ContatosController(ServicoContato servicoContato, IMapper mapeadorContatos)
        {
            this.servicoContato = servicoContato;
            this.mapeadorContatos = mapeadorContatos;
        }

        [HttpGet]
        public ActionResult<List<ListarContatosViewModel>> SelecionarTodos()
        {
            var contatoResult = servicoContato.SelecionarTodos();

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return RetornarOkComMap<List<Contato>, List<ListarContatosViewModel>>(contatoResult);
        }

        [HttpGet("visualizar-completo/{id:guid}")]
        public ActionResult<VisualizarContatoViewModel> SelecionarPorId(Guid id)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.IsFailed && RegistroNaoEncontrado(contatoResult))
                return NotFound(contatoResult);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return RetornarOkComMap<Contato, VisualizarContatoViewModel>(contatoResult);
        }

        [HttpPost]
        public ActionResult<FormsContatoViewModel> Inserir(InserirContatoViewModel contatoVM)
        {
            var contato = mapeadorContatos.Map<Contato>(contatoVM);

            var contatoResult = servicoContato.Inserir(contato);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return RetornarOkSemMap<InserirContatoViewModel>(contatoVM);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsContatoViewModel> Editar(Guid id, EditarContatoViewModel contatoVM)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.IsFailed && RegistroNaoEncontrado(contatoResult))
                return NotFound(contatoResult);

            var contato = mapeadorContatos.Map(contatoVM, contatoResult.Value);

            contatoResult = servicoContato.Editar(contato);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return RetornarOkSemMap<EditarContatoViewModel>(contatoVM);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var contatoResult = servicoContato.Excluir(id);

            if (contatoResult.IsFailed && RegistroNaoEncontrado<Contato>(contatoResult))
                return NotFound(contatoResult);

            if (contatoResult.IsFailed)
                return InternalError<Contato>(contatoResult);

            return NoContent();
        }

        #region Métodos Privados

        public ActionResult RetornarOkComMap<TInput, TOutput>(Result<TInput> contatoResult)
        {
            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<TOutput>(contatoResult.Value)
            });
        }

        public ActionResult RetornarOkSemMap<T>(Result<T> contatoResult)
        {
            return Ok(new
            {
                sucesso = true,
                dados = contatoResult
            });
        }
        #endregion
    }
}
