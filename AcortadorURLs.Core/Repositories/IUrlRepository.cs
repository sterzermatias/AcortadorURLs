using AcortadorURLs.Core.Entities;
using System.Threading.Tasks;

namespace AcortadorURLs.Core.Repositories
{
    public interface IUrlRepository
    {
        Task<Url?> ObtenerPorCodigoAsync(string codigo);
        Task AgregarAsync(Url url);
        Task ActualizarAsync(Url url); 
    }
}