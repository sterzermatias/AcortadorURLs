using System;

namespace AcortadorURLs.Core.Services
{
    public class GeneradorCodigoHexadecimal : IGeneradorCodigoStrategy
    {
        public string Generar()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
