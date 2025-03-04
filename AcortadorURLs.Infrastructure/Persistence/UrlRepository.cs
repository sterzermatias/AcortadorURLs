using AcortadorURLs.Core.Entities;
using AcortadorURLs.Core.ValueObjects;
using AcortadorURLs.Core.Repositories;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AcortadorURLs.Infrastructure.Persistence
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ConcurrentDictionary<string, Url> _urls = new();

        public async Task<Url?> ObtenerPorCodigoAsync(string codigo)
        {
            _urls.TryGetValue(codigo, out var url);
            return url;
        }

        public async Task AgregarAsync(Url url)
        {
            _urls[url.CodigoCorto.ToString()] = url;
        }
       
        public async Task ActualizarAsync(Url url)
        {
            if (_urls.ContainsKey(url.CodigoCorto.ToString()))
            {
                _urls[url.CodigoCorto.ToString()] = url; 
            }
        }
    }
}
