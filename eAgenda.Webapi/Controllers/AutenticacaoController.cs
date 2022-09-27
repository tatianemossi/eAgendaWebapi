using AutoMapper;
using eAgenda.Aplicacao.ModuloAutenticacao;
using eAgenda.Dominio.ModuloAutenticacao;
using eAgenda.Webapi.ViewModels.ModuloAutenticacao;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/conta")]
    [ApiController]
    public class AutenticacaoController : eAgendaControllerBase
    {
        private readonly ServicoAutenticacao servicoAutenticacao;
        private readonly IMapper mapeadorUsuario;

        public AutenticacaoController(ServicoAutenticacao servicoAutenticacao, IMapper mapeadorUsuario)
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.mapeadorUsuario = mapeadorUsuario;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarUsuario(RegistrarUsuarioViewModel usuarioVM)
        {
            var usuario = mapeadorUsuario.Map<Usuario>(usuarioVM);

            var usuarioResult =  await servicoAutenticacao.RegistrarUsuario(usuario, usuarioVM.Senha);

            if (usuarioResult.IsFailed)
                return InternalError(usuarioResult);

            return Ok(new
            {
                sucesso = true,
                dados = usuarioVM
            });
        }
    }
}
