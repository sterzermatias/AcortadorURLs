namespace AcortadorURLs.Appliaction.DTOs
{
    public class ResultadoAcortador
    {
        public string CodigoCorto { get; }
        public bool YaExistia { get; }

        public ResultadoAcortador(string codigoCorto, bool yaExistia)
        {
            CodigoCorto = codigoCorto;
            YaExistia = yaExistia;
        }
    }
}
