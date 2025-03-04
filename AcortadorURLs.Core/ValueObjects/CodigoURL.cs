namespace AcortadorURLs.Core.ValueObjects
{
    public class CodigoURL
    {
        public string Valor { get; }

        public CodigoURL(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || valor.Length < 6)
                throw new ArgumentException("El código debe tener al menos 6 caracteres.");

            Valor = valor;
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
