using System.ComponentModel;

namespace eAgenda.Dominio.ModuloCompromisso
{
    public enum TipoLocalCompromissoEnum : int
    {
        [Description("Remoto")]
        Remoto = 0,

        [Description("Presencial")]
        Presencial = 1
    }
}
