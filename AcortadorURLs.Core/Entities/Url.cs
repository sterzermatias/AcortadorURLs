using AcortadorURLs.Core.ValueObjects;

namespace AcortadorURLs.Core.Entities
{
    public class Url
    {
        public Guid Id { get; private set; }
        public string UrlOriginal { get; private set; }
        public CodigoURL CodigoCorto { get; private set; }
        public int Clics { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        public Url(string urlOriginal, CodigoURL codigoCorto)
        {
            Id = Guid.NewGuid();
            UrlOriginal = urlOriginal;
            CodigoCorto = codigoCorto;
            Clics = 0;
            FechaCreacion = DateTime.UtcNow;
        }

        public void IncrementarClics()
        {
            Clics++;
        }
    }
}
