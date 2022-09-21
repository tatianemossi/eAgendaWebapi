using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;
using AutoMapper;

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
        public List<ListarContatosViewModel> SelecionarTodos()
        {
            var contatoResult = servicoContato.SelecionarTodos();

            if (contatoResult.IsSuccess)
                return mapeadorContatos.Map<List<ListarContatosViewModel>>(contatoResult.Value);

            return null;
        }

        [HttpGet("visualizar-completo/{id:guid}")]
        public VisualizarContatoViewModel SelecionarPorId(Guid id)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.IsSuccess)
                return mapeadorContatos.Map<VisualizarContatoViewModel>(contatoResult.Value);

            return null;
        }

        [HttpPost]
        public FormsContatoViewModel Inserir(InserirContatoViewModel contatoVM)
        {
            var contato = mapeadorContatos.Map<Contato>(contatoVM);

            var contatoResult = servicoContato.Inserir(contato);

            if (contatoResult.IsSuccess)
                return contatoVM;

            return null;
        }

        [HttpPut("{id:guid}")]
        public FormsContatoViewModel Editar(Guid id, EditarContatoViewModel contatoVM)
        {
            var contatoSelecionado = servicoContato.SelecionarPorId(id).Value;

            var contato = mapeadorContatos.Map(contatoVM, contatoSelecionado);

            var contatoResult = servicoContato.Editar(contato);

            if (contatoResult.IsSuccess)
                return contatoVM;

            return null;
        }

        [HttpDelete("{id:guid}")]
        public void Excluir(Guid id)
        {
            servicoContato.Excluir(id);
        }
    }
}
