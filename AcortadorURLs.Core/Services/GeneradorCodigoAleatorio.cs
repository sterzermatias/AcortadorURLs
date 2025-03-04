using System;
using System.Linq;

namespace AcortadorURLs.Core.Services
{
    public class GeneradorCodigoAleatorio : IGeneradorCodigoStrategy
    {
        private static readonly Random _random = new();

        public string Generar()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, 6)
                .Select(_ => caracteres[_random.Next(caracteres.Length)])
                .ToArray());
        }
    }
}
