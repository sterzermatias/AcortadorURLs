using AcortadorURLs.Core.Entities;
using System.Threading.Tasks;

namespace AcortadorURLs.Core.Repositories
{
    public interface IUrlRepository
    {
        Task<Url?> ObtenerPorCodigoAsync(string codigo);
        Task<Url?> ObtenerPorUrlAsync(string urlOriginal);
        Task AgregarAsync(Url url);
        Task ActualizarAsync(Url url); 
    }
}