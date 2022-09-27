using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Dominio;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Infra.Orm;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using eAgenda.Webapi.Config.AutoMapperConfig;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Aplicacao.ModuloContato;
using Microsoft.AspNetCore.Mvc;
using eAgenda.Webapi.Filters;
using eAgenda.Aplicacao.ModuloAutenticacao;
using Microsoft.AspNetCore.Identity;
using eAgenda.Dominio.ModuloAutenticacao;
using System;

namespace eAgenda.Webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.SuppressModelStateInvalidFilter = true;
            });

            services.AddAutoMapper(config =>
            {
                config.AddProfile<TarefaProfile>();
                config.AddProfile<ContatoProfile>();
                config.AddProfile<UsuarioProfile>();
            });
            services.AddSingleton((x) => new ConfiguracaoAplicacaoeAgenda().ConnectionStrings);

            services.AddScoped<eAgendaDbContext>();

            services.AddIdentity<Usuario, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<eAgendaDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<UserManager<Usuario>>();

            services.AddScoped<IContextoPersistencia, eAgendaDbContext>();

            services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();
            services.AddTransient<ServicoTarefa>();

            services.AddScoped<IRepositorioContato, RepositorioContatoOrm>();
            services.AddTransient<ServicoContato>();

            services.AddTransient<ServicoAutenticacao>();

            services.AddControllers(config =>
            {
                config.Filters.Add(new ValidarViewModelActionFilter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eAgenda.Webapi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eAgenda.Webapi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
