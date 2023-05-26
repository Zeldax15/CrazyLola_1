using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CrazyLola.Models;
using CrazyLola.Servicios.Contrato;

namespace CrazyLola.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly CrazylolaContext _CrazylolaContext;
        public UsuarioService(CrazylolaContext CrazyLolaContext)
        {
            _CrazylolaContext = CrazyLolaContext;
        }

        public async Task<Usuario> GetUsuario(string nombre, string clave)
        {
            Usuario usuario_encontrado = await _CrazylolaContext.Usuarios.Where(u => u.NombreUsuario == nombre && u.Clave == clave)
                 .FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _CrazylolaContext.Usuarios.Add(modelo);
            await _CrazylolaContext.SaveChangesAsync();
            return modelo;
        }
    }
}
