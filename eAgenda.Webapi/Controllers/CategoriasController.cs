using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : eAgendaControllerBase
    {
        private readonly ServicoCategoria servicoCategoria;
        private readonly IMapper mapeadorCategoria;

        public CategoriasController(ServicoCategoria servicoCategoria, IMapper mapeadorCategoria)
        {
            this.servicoCategoria = servicoCategoria;
            this.mapeadorCategoria = mapeadorCategoria;
        }

        [HttpGet]
        public ActionResult<List<ListarCategoriaViewModel>> SelecionarTodos()
        {
            var categoriaResult = servicoCategoria.SelecionarTodos();

            if (categoriaResult.IsFailed)
                return InternalError(categoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<List<ListarCategoriaViewModel>>(categoriaResult.Value)
            });
        }

        [HttpGet("vizualizacao-completa/{id:guid}")]
        public ActionResult<VisualizarCategoriaViewModel> SelecionarPorId(Guid id)
        {
            var categoriaResult = servicoCategoria.SelecionarPorId(id);

            if (categoriaResult.IsFailed && RegistroNaoEncontrado(categoriaResult))
                return NotFound(categoriaResult);

            if (categoriaResult.IsFailed)
                return InternalError(categoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<VisualizarCategoriaViewModel>(categoriaResult.Value)
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<FormsCategoriaViewModel> SelecionarCategoriaPorId(Guid id)
        {
            var categoriaResult = servicoCategoria.SelecionarPorId(id);

            if (categoriaResult.IsFailed && RegistroNaoEncontrado(categoriaResult))
                return NotFound(categoriaResult);

            if (categoriaResult.IsFailed)
                return InternalError(categoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<FormsCategoriaViewModel>(categoriaResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<FormsCategoriaViewModel> Inserir(FormsCategoriaViewModel categoriaVM)
        {
            var categoria = mapeadorCategoria.Map<Categoria>(categoriaVM);

            categoria.UsuarioId = UsuarioLogado.Id;

            var categoriaResult = servicoCategoria.Inserir(categoria);

            if (categoriaResult.IsFailed)
                return InternalError(categoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = categoriaVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsCategoriaViewModel> Editar(Guid id, FormsCategoriaViewModel categoriaVM)
        {
            var categoriaResult = servicoCategoria.SelecionarPorId(id);

            if (categoriaResult.IsFailed && RegistroNaoEncontrado(categoriaResult))
                return NotFound(categoriaResult);

            var categoria = mapeadorCategoria.Map(categoriaVM, categoriaResult.Value);

            categoriaResult = servicoCategoria.Editar(categoria);

            if (categoriaResult.IsFailed)
                return InternalError(categoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = categoriaVM
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var categoriaResult = servicoCategoria.Excluir(id);

            if (categoriaResult.IsFailed && RegistroNaoEncontrado<Categoria>(categoriaResult))
                return NotFound(categoriaResult);

            if (categoriaResult.IsFailed)
                return InternalError<Categoria>(categoriaResult);

            return NoContent();
        }
    }
}
