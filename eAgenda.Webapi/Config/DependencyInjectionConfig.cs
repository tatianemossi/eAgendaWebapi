﻿using eAgenda.Aplicacao.ModuloAutenticacao;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Dominio;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Infra.Orm;
using Microsoft.Extensions.DependencyInjection;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Aplicacao.ModuloCompromisso;

namespace eAgenda.Webapi.Config
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigurarInjecaoDependencia(this IServiceCollection services)
        {
            services.AddSingleton((x) => new ConfiguracaoAplicacaoeAgenda().ConnectionStrings);

            services.AddScoped<eAgendaDbContext>();

            services.AddScoped<IContextoPersistencia, eAgendaDbContext>();

            services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();
            services.AddTransient<ServicoTarefa>();

            services.AddScoped<IRepositorioContato, RepositorioContatoOrm>();
            services.AddTransient<ServicoContato>();

            services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoOrm>();
            services.AddTransient<ServicoCompromisso>();

            services.AddTransient<ServicoAutenticacao>();
        }
    }
}
