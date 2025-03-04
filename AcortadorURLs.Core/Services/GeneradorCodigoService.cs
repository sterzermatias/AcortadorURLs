namespace AcortadorURLs.Core.Services
{
    public class GeneradorCodigoService
    {
        private IGeneradorCodigoStrategy _strategy;

        public GeneradorCodigoService(IGeneradorCodigoStrategy strategy)
        {
            _strategy = strategy;
        }

        public void EstablecerEstrategia(IGeneradorCodigoStrategy strategy)
        {
            _strategy = strategy;
        }

        public string GenerarCodigo()
        {
            return _strategy.Generar();
        }
    }
}
