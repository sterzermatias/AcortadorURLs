using AcortadorURLs.Core.Entities;
using AcortadorURLs.Core.Repositories;
using AcortadorURLs.Core.Services;
using AcortadorURLs.Core.ValueObjects;
using System;
using System.Threading.Tasks;

namespace AcortadorURLs.Application.UseCases
{
    public class AcortarURL
    {
        private readonly IUrlRepository _urlRepository;
        private readonly GeneradorCodigoService _generadorCodigoService;

        public AcortarURL(IUrlRepository urlRepository, GeneradorCodigoService generadorCodigoService)
        {
            _urlRepository = urlRepository;
            _generadorCodigoService = generadorCodigoService;
        }

        public async Task<string> Ejecutar(string urlOriginal)
        {
            string codigoGenerado = _generadorCodigoService.Generar();
            var url = new Url(urlOriginal, new CodigoURL(codigoGenerado));

            await _urlRepository.AgregarAsync(url);

            return codigoGenerado;
        }
    }
}
