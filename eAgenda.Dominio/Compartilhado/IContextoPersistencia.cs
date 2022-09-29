namespace eAgenda.Dominio.Compartilhado
{
    public interface IContextoPersistencia
    {
        void DesfazerAlteracoes();

        void GravarDados();
    }
}
