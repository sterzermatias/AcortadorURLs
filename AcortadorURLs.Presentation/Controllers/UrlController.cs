using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AcortadorURLs.Application.UseCases;
using AcortadorURLs.Core.Services;
using AcortadorURLs.Core.Repositories;
using AcortadorURLs.Appliaction.DTOs;
using AcortadorURLs.Presentation.DTOs;


namespace AcortadorURLs.Presentation.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlController : ControllerBase
    {
        private readonly AcortarURL _acortarURL;
        private readonly ObtenerURL _obtenerURL;
        private readonly IUrlRepository _urlRepository;
        private readonly GeneradorCodigoService  _generadorCodigoService;
        public UrlController(AcortarURL acortarURL, ObtenerURL obtenerURL, IUrlRepository urlRepository, GeneradorCodigoService  generadorCodigoService)
        {
            _acortarURL = acortarURL;
            _urlRepository = urlRepository;
            _obtenerURL = obtenerURL;
            _generadorCodigoService = generadorCodigoService;
        }

        [HttpPost("acortar")]
        public async Task<IActionResult> Acortar([FromBody] AcortarUrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
                return BadRequest("La URL no puede estar vacía.");
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
                return BadRequest(new { mensaje = "La URL proporcionada no es válida." });
            try
            {
                var resultado = await _acortarURL.Ejecutar(request.Url, request.CodigoPersonalizado);
                return Ok(new 
                {
                    mensaje = resultado.YaExistia ? "Esta URL ya fue acortada anteriormente." : "URL acortada con éxito.",
                    urlCorta = $"https://acortar.io/{resultado.CodigoCorto}"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
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
        [HttpPost("cambiar-estrategia")]
        public IActionResult CambiarEstrategia([FromBody] EstrategiaCodigoRequest request)
        {
            if (request.Estrategia.Equals("aleatorio", StringComparison.OrdinalIgnoreCase))
                _generadorCodigoService.EstablecerEstrategia(new GeneradorCodigoAleatorio());
            else if (request.Estrategia.Equals("hexadecimal", StringComparison.OrdinalIgnoreCase))
                _generadorCodigoService.EstablecerEstrategia(new GeneradorCodigoHexadecimal());
            else
                return BadRequest(new { mensaje = "Estrategia no válida. Usa 'aleatorio' o 'hexadecimal'." });

            return Ok(new { mensaje = $"Estrategia cambiada a {request.Estrategia}." });
        }

    }
}