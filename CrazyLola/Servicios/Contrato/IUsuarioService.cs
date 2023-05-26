using CrazyLola.Models;
using Microsoft.EntityFrameworkCore;


namespace CrazyLola.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string nombre, string clave);
        Task<Usuario> SaveUsuario(Usuario modelo);

    }

}
