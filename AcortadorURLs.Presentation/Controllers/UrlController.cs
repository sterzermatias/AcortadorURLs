using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AcortadorURLs.Application.UseCases;
using AcortadorURLs.Core.Repositories;

namespace AcortadorURLs.Presentation.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlController : ControllerBase
    {
        private readonly AcortarURL _acortarURL;
        private readonly ObtenerURL _obtenerURL;
        private readonly IUrlRepository _urlRepository;
        public UrlController(AcortarURL acortarURL, ObtenerURL obtenerURL, IUrlRepository urlRepository)
        {
            _acortarURL = acortarURL;
            _urlRepository = urlRepository;
            _obtenerURL = obtenerURL;
        }

        [HttpPost("acortar")]
        public async Task<IActionResult> Acortar([FromBody] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest("La URL no puede estar vacía.");
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                return BadRequest(new { mensaje = "La URL proporcionada no es válida." });
            try
            {
                var urlExistente = await _urlRepository.ObtenerPorCodigoAsync(url);
                if (urlExistente != null)
                    return Ok(new { mensaje = "Esta URL ya fue acortada.", urlCorta = $"https://acortar.io/{urlExistente.CodigoCorto}" });

                // Generar la URL corta
                var codigo = await _acortarURL.Ejecutar(url);
                return Ok(new { mensaje = "URL acortada con éxito", urlCorta = $"https://acortar.io/{codigo}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error en el servidor.", error = ex.Message });
            }
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObtenerURL(string codigo)
        {
            var urlOriginal = await _obtenerURL.Ejecutar(codigo);

            if (urlOriginal == null)
                return NotFound(new { mensaje = "Código no encontrado." });

            return Redirect(urlOriginal); 
        }

        [HttpGet("{codigo}/stats")]
        public async Task<IActionResult> ObtenerEstadisticas(string codigo)
        {
            var url = await _obtenerURL.ObtenerEntidad(codigo); 
            if (url == null)
                return NotFound(new { mensaje = "Código no encontrado." });

            return Ok(new { urlOriginal = url.UrlOriginal, clics = url.Clics });
        }
    }
}