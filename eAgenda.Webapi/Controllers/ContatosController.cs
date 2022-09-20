using eAgenda.Infra.Configs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Infra.Orm;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Dominio.ModuloContato;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        private readonly ServicoContato servicoContato;

        public ContatosController()
        {
            var config = new ConfiguracaoAplicacaoeAgenda();

            var eAgendaDbContext = new eAgendaDbContext(config.ConnectionStrings);
            var repositorioContato = new RepositorioContatoOrm(eAgendaDbContext);
            servicoContato = new ServicoContato(repositorioContato, eAgendaDbContext);
        }

        [HttpGet]
        public List<Contato> SelecionarTodos()
        {
            var contatoResult = servicoContato.SelecionarTodos();

            if (contatoResult.IsSuccess)
                return contatoResult.Value;

            return null;
        }

        [HttpGet("{id:guid}")]
        public Contato SelecionarPorId(Guid id)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.IsSuccess)
                return contatoResult.Value;

            return null;
        }

        [HttpPost]
        public Contato Inserir(Contato novoContato) 
        {
            var contatoResult = servicoContato.Inserir(novoContato);

            if (contatoResult.IsSuccess)
                return contatoResult.Value;

            return null;
        }

        [HttpPut("{id:guid}")]
        public Contato Editar(Guid id, Contato contato)
        {
            var contatoEditado = servicoContato.SelecionarPorId(id).Value;
            contatoEditado.Nome = contato.Nome;
            contatoEditado.Email = contato.Email;
            contatoEditado.Telefone = contato.Telefone;
            contatoEditado.Empresa = contato.Empresa;
            contatoEditado.Cargo = contato.Cargo;

            var contatoResult = servicoContato.Editar(contatoEditado);

            if (contatoResult.IsSuccess)
                return contatoResult.Value;

            return null;
        }

        [HttpDelete("{id:guid}")]
        public void Excluir(Guid id)
        {
            servicoContato.Excluir(id);
        }
    }
}
