using eAgenda.Dominio.Compartilhado;
using System;
using System.Collections.Generic;

namespace eAgenda.Dominio.ModuloDespesa
{
    public interface IRepositorioDespesa : IRepositorio<Despesa>
    {
        void Inserir(Despesa novoRegistro, List<Categoria> categoriasMarcadas);

        void Editar(Despesa novoRegistro, List<Categoria> categoriasMarcadas, List<Categoria> categoriasDesmarcadas);

        List<Despesa> SelecionarTodosPeloUsuarioId(Guid usuarioId);
    }
}
