﻿using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloAutenticacao;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloTarefa;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class TarefaProfile : Profile
    {
        public TarefaProfile()
        {
            ConverterDeEntidadeParaViewModel();
            ConverterDeViewModelParaEntidade();
        }

        private void ConverterDeViewModelParaEntidade()
        {
            CreateMap<InserirTarefaViewModel, Tarefa>()
                .ForMember(destino => destino.UsuarioId, opt => opt.MapFrom<UsuarioResolver>())
                .ForMember(destino => destino.Itens, opt => opt.Ignore())
                .AfterMap((viewModel, tarefa) =>
                {
                    if (viewModel.Itens == null)
                        return;

                    foreach (var itemVM in viewModel.Itens)
                    {
                        var item = new ItemTarefa();

                        item.Titulo = itemVM.Titulo;

                        tarefa.AdicionarItem(item);
                    }
                });

            CreateMap<EditarTarefaViewModel, Tarefa>()
                .ForMember(destino => destino.Itens, opt => opt.Ignore())
                .AfterMap((viewModel, tarefa) =>
            {
                foreach (var itemVM in viewModel.Itens)
                {
                    if (itemVM.Concluido)
                        tarefa.ConcluirItem(itemVM.Id);
                    else
                        tarefa.MarcarPendente(itemVM.Id);
                }

                foreach (var itemVM in viewModel.Itens)
                {
                    if (itemVM.Status == StatusItemTarefa.Adicionado)
                    {
                        var item = new ItemTarefa(itemVM.Titulo);
                        tarefa.AdicionarItem(item);
                    }
                    else if (itemVM.Status == StatusItemTarefa.Removido)
                        tarefa.RemoverItem(itemVM.Id);
                }
            });
        }

        private void ConverterDeEntidadeParaViewModel()
        {
            CreateMap<Tarefa, ListarTarefasViewModel>()
                .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                .ForMember(destino => destino.Situacao, opt =>
                    opt.MapFrom(origem => origem.PercentualConcluido == 100 ? "Concluída" : "Pendente"));

            CreateMap<Tarefa, VisualizarTarefaViewModel>()
                .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                .ForMember(destino => destino.Situacao, opt =>
                    opt.MapFrom(origem => origem.PercentualConcluido == 100 ? "Concluída" : "Pendente"))
                .ForMember(destino => destino.QuantidadeItens, opt => opt.MapFrom(origem => origem.Itens.Count));

            CreateMap<ItemTarefa, VisualizarItemTarefaViewModel>()
                .ForMember(destino => destino.Situacao, opt =>
                    opt.MapFrom(origem => origem.Concluido ? "Concluído" : "Pendente"));
        }
    }
}
