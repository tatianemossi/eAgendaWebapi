﻿using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Webapi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ServicoTarefa servicoTarefa;

        public TarefasController()
        {
            var config = new ConfiguracaoAplicacaoeAgenda();

            var eAgendaDbContext = new eAgendaDbContext(config.ConnectionStrings);
            var repositorioTarefa = new RepositorioTarefaOrm(eAgendaDbContext);
            servicoTarefa = new ServicoTarefa(repositorioTarefa, eAgendaDbContext);
        }

        [HttpGet]
        public List<ListarTarefasViewModel> SelecionarTodos()
        {
            var tarefaResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos);

            if (tarefaResult.IsSuccess)
            {
                var tarefasGravadas = tarefaResult.Value;

                var listagemTarefas = new List<ListarTarefasViewModel>();

                foreach (var item in tarefasGravadas)
                {
                    var tarefaVM = new ListarTarefasViewModel
                    {
                        Id = item.Id,
                        Titulo = item.Titulo,
                        Prioridade = item.Prioridade.GetDescription(),
                        Situacao = item.PercentualConcluido == 100 ? "Concluída" : "Pendente"
                    };

                    listagemTarefas.Add(tarefaVM);
                }

                return listagemTarefas;
            }

            return null;
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public VisualizarTarefaViewModel SelecionarPorId(Guid id) //4AD3BC00-9546-4013-EAB7-08DA9A6BD2BC
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsSuccess)
            {
                var tarefaVM = new VisualizarTarefaViewModel();

                var tarefa = tarefaResult.Value;

                tarefaVM.Titulo = tarefa.Titulo;
                tarefaVM.DataCriacao = tarefa.DataCriacao;
                tarefaVM.DataConclusao = tarefa.DataConclusao;
                tarefaVM.Prioridade = tarefa.Prioridade.GetDescription();
                tarefaVM.Situacao = tarefa.PercentualConcluido == 100 ? "Concluída" : "Pendente";
                tarefaVM.QuantidadeItens = tarefa.Itens.Count;
                tarefaVM.PercentualConcluido = tarefa.PercentualConcluido;

                foreach (var itemTarefa in tarefa.Itens)
                {
                    var itemVM = new VisualizarItemTarefaViewModel();
                    itemVM.Titulo = itemTarefa.Titulo;
                    itemVM.Situacao = itemTarefa.Concluido ? "Concluído" : "Pendente";

                    tarefaVM.Itens.Add(itemVM);
                }

                return tarefaVM;
            }

            return null;
        }

        [HttpPost]
        public FormsTarefasViewModel Inserir(FormsTarefasViewModel tarefaVM) //databinding - modelbinder
        {
            var tarefa = new Tarefa();

            tarefa.Titulo = tarefaVM.Titulo;
            tarefa.Prioridade = tarefaVM.Prioridade;

            foreach (var itemVM in tarefaVM.Itens)
            {
                var item = new ItemTarefa();

                item.Titulo = itemVM.Titulo;

                tarefa.AdicionarItem(item);
            }

            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsSuccess)
            {
                return tarefaVM;
            }

            return null;
        }

        [HttpPut("{id:guid}")]
        public FormsTarefasViewModel Editar(Guid id, FormsTarefasViewModel tarefaVM)
        {
            var tarefaEditada = servicoTarefa.SelecionarPorId(id).Value;

            tarefaEditada.Titulo = tarefaVM.Titulo;
            tarefaEditada.Prioridade = tarefaVM.Prioridade;

            foreach (var itemVM in tarefaVM.Itens)
            {
                if (itemVM.Concluido)
                    tarefaEditada.ConcluirItem(itemVM.Id);
                else
                    tarefaEditada.MarcarPendente(itemVM.Id);
            }

            foreach (var itemVM in tarefaVM.Itens)
            {
                if (itemVM.Status == StatusItemTarefa.Adicionado)
                {
                    var item = new ItemTarefa(itemVM.Titulo);
                    tarefaEditada.AdicionarItem(item);
                }
                else if (itemVM.Status == StatusItemTarefa.Removido)
                    tarefaEditada.RemoverItem(itemVM.Id);

            }

            var tarefaResult = servicoTarefa.Editar(tarefaEditada);

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
