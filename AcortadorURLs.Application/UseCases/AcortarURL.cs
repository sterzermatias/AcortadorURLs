using AcortadorURLs.Core.Entities;
using AcortadorURLs.Core.Repositories;
using AcortadorURLs.Core.Services;
using AcortadorURLs.Core.ValueObjects;
using System;
using System.Threading.Tasks;
using AcortadorURLs.Appliaction.DTOs;


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

        public async Task<ResultadoAcortador> Ejecutar(string urlOriginal, string? codigoPersonalizado)
        {
            var urlExistente = await _urlRepository.ObtenerPorUrlAsync(urlOriginal);
            if (urlExistente != null)
            {
                return new ResultadoAcortador(urlExistente.CodigoCorto.ToString(), true);
            }

            var codigo = !string.IsNullOrEmpty(codigoPersonalizado)
            ? await ValidarCodigoPersonalizado(codigoPersonalizado)
            : _generadorCodigoService.GenerarCodigo();
            await CrearYGuardarUrl(urlOriginal, codigo);
            return new ResultadoAcortador(codigo, false);   
        }

    private async Task<string> ValidarCodigoPersonalizado(string codigoPersonalizado)
    {
        if (await _urlRepository.ObtenerPorCodigoAsync(codigoPersonalizado) != null)
        {
            throw new InvalidOperationException("El código corto elegido ya está en uso.");
        }
        return codigoPersonalizado;
    }
    private async Task<string> CrearYGuardarUrl(string urlOriginal, string codigo)
    {
        var url = new Url(urlOriginal, new CodigoURL(codigo));
        await _urlRepository.AgregarAsync(url);
        return codigo;
    }
    }
}
