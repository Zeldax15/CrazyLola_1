using Microsoft.AspNetCore.Mvc;
using CrazyLola.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using CrazyLola.Models;
using Crazylola.Recursos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;


namespace CrazyLola.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);

            Usuario usuario_creado = await _usuarioServicio.SaveUsuario(modelo);

            if (usuario_creado.IdUsuario > 0)
                return RedirectToAction("IniciarSesion", "Inicio");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        public IActionResult VerDocumento()
        {
            // Lógica para obtener la ruta del documento
            string rutaDocumento = "C:/Users/Dany0/Documents/CVOscarDanielValleHernandez.docx"; // Reemplaza con la ruta correcta

            // Verifica que el archivo exista
            if (!System.IO.File.Exists(rutaDocumento))
            {
                return NotFound();
            }

            // Obtén el tipo MIME del archivo
            var proveedorMIME = new FileExtensionContentTypeProvider();
            if (!proveedorMIME.TryGetContentType(rutaDocumento, out string mimeType))
            {
                mimeType = "application/octet-stream";
            }

            // Lee el contenido del archivo
            byte[] contenido = System.IO.File.ReadAllBytes(rutaDocumento);

            // Retorna el archivo como un archivo para descargar en el navegador
            return File(contenido, mimeType);
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string nombre, string clave)
        {

            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(nombre, Utilidades.EncriptarClave(clave));

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
