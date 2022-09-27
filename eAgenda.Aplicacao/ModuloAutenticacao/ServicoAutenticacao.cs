using eAgenda.Dominio.ModuloAutenticacao;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloAutenticacao
{
    public class ServicoAutenticacao : ServicoBase<Usuario, ValidadorUsuario>
    {
        private readonly UserManager<Usuario> userManager;

        public ServicoAutenticacao(UserManager<Usuario> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Result<Usuario>> RegistrarUsuario(Usuario usuario, string senha)
        {
            Log.Logger.Debug("Tentando registrar usuário... {@u}", usuario);

            var resultado = Validar(usuario);
            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            try
            {
                var usuarioResult = await userManager.CreateAsync(usuario, senha);

                if (usuarioResult.Succeeded == false)
                {
                    var erros = usuarioResult.Errors
                        .Select(identityErro => new Error(identityErro.Description));
                     
                    return Result.Fail(erros);
                }

                Log.Logger.Information("Usuário {UsuaruiId} registrado com sucesso", usuario.Id);
            }
            catch (Exception ex)
            {
                //contextoPersistencia.DesfazerAlteracoes();
                string msgErro = "Falha no sistema ao tentar registrar o usuário";

                Log.Logger.Error(ex, msgErro + " {UsuarioId}", usuario.Id);

                return Result.Fail(msgErro);
            }

            return Result.Ok(usuario);
        }
    }
}
