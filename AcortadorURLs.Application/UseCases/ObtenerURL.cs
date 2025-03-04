using AcortadorURLs.Core.Entities;
using AcortadorURLs.Core.Repositories;
using AcortadorURLs.Core.Services;
using AcortadorURLs.Core.ValueObjects;
using System;
using System.Threading.Tasks;

namespace AcortadorURLs.Application.UseCases
{
    public class ObtenerURL
    {
        private readonly IUrlRepository _urlRepository;

        public ObtenerURL(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<string?> Ejecutar(string codigo)
        {
            var url = await _urlRepository.ObtenerPorCodigoAsync(codigo);
            if (url == null)
                return null;

            url.IncrementarClics(); 
            await _urlRepository.ActualizarAsync(url);

            return url.UrlOriginal;
        }
        public async Task<Url?> ObtenerEntidad(string codigo)
        {
            return await _urlRepository.ObtenerPorCodigoAsync(codigo);
        }
    }
}
