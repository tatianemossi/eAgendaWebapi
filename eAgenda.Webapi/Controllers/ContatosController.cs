using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;
using AutoMapper;
using FluentResults;
using System.Net;
using System.Linq;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
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
                return RetornaErro(HttpStatusCode.InternalServerError, contatoResult.Errors.Select(x => x.Message));

            return RetornarOkComMap<List<Contato>, List<ListarContatosViewModel>>(contatoResult);
        }

        [HttpGet("visualizar-completo/{id:guid}")]
        public ActionResult<VisualizarContatoViewModel> SelecionarPorId(Guid id)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.Errors.Any(x => x.Message.Contains("não encontrado")))
                return RetornaErro(HttpStatusCode.NotFound, contatoResult.Errors.Select(x => x.Message));

            if (contatoResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, contatoResult.Errors.Select(x => x.Message));

            return RetornarOkComMap<Contato, VisualizarContatoViewModel>(contatoResult);
        }

        [HttpPost]
        public ActionResult<FormsContatoViewModel> Inserir(InserirContatoViewModel contatoVM)
        {
            var listaErros = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);

            if (listaErros.Any())
                return RetornaErro(HttpStatusCode.BadRequest, listaErros);

            var contato = mapeadorContatos.Map<Contato>(contatoVM);

            var contatoResult = servicoContato.Inserir(contato);

            if (contatoResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, contatoResult.Errors.Select(x => x.Message));

            return RetornarOkSemMap<InserirContatoViewModel>(contatoVM);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsContatoViewModel> Editar(Guid id, EditarContatoViewModel contatoVM)
        {
            var listaErros = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);

            if (listaErros.Any())
                return RetornaErro(HttpStatusCode.BadRequest, listaErros);

            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.Errors.Any(x => x.Message.Contains("não encontrado")))
                return RetornaErro(HttpStatusCode.NotFound, contatoResult.Errors.Select(x => x.Message));

            var contato = mapeadorContatos.Map(contatoVM, contatoResult.Value);

            contatoResult = servicoContato.Editar(contato);

            if (contatoResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, contatoResult.Errors.Select(x => x.Message));

            return RetornarOkSemMap<EditarContatoViewModel>(contatoVM);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var contatoResult = servicoContato.Excluir(id);

            if (contatoResult.Errors.Any(x => x.Message.Contains("não encontrado")))
                return RetornaErro(HttpStatusCode.NotFound, contatoResult.Errors.Select(x => x.Message));

            if (contatoResult.IsFailed)
                return RetornaErro(HttpStatusCode.InternalServerError, contatoResult.Errors.Select(x => x.Message));

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
    }
}
