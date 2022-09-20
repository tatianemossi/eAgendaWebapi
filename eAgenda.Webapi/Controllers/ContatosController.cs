using eAgenda.Infra.Configs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using eAgenda.Infra.Orm;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;

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
        public List<ListarContatosViewModel> SelecionarTodos()
        {
            var contatoResult = servicoContato.SelecionarTodos();

            if (contatoResult.IsSuccess)
            {
                var contatosGravados = contatoResult.Value;

                var listagemContatos = new List<ListarContatosViewModel>();

                foreach (var item in contatosGravados)
                {
                    var contatoVM = new ListarContatosViewModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Telefone = item.Telefone,
                        Empresa = item.Empresa
                    };

                    listagemContatos.Add(contatoVM);
                }

                return listagemContatos;
            }

            return null;
        }

        [HttpGet("visualizar-completo/{id:guid}")]
        public VisualizarContatoViewModel SelecionarPorId(Guid id)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.IsSuccess)
            {
                var contatoVM = new VisualizarContatoViewModel();

                var contato = contatoResult.Value;

                contatoVM.Nome = contato.Nome;
                contatoVM.Email = contato.Email;
                contatoVM.Telefone = contato.Telefone;
                contatoVM.Empresa = contato.Empresa;
                contatoVM.Cargo = contato.Cargo;

                return contatoVM;
            }

            return null;
        }

        [HttpPost]
        public FormsContatoViewModel Inserir(FormsContatoViewModel contatoVM)
        {
            var contato = new Contato();

            contato.Nome = contatoVM.Nome;
            contato.Email = contatoVM.Email;
            contato.Telefone = contatoVM.Telefone;
            contato.Empresa = contatoVM.Empresa;
            contato.Cargo = contatoVM.Cargo;

            var contatoResult = servicoContato.Inserir(contato);

            if (contatoResult.IsSuccess)
                return contatoVM;

            return null;
        }

        [HttpPut("{id:guid}")]
        public FormsContatoViewModel Editar(Guid id, FormsContatoViewModel contatoVM)
        {
            var contatoEditado = servicoContato.SelecionarPorId(id).Value;

            contatoEditado.Nome = contatoVM.Nome;
            contatoEditado.Email = contatoVM.Email;
            contatoEditado.Telefone = contatoVM.Telefone;
            contatoEditado.Empresa = contatoVM.Empresa;
            contatoEditado.Cargo = contatoVM.Cargo;

            var contatoResult = servicoContato.Editar(contatoEditado);

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
