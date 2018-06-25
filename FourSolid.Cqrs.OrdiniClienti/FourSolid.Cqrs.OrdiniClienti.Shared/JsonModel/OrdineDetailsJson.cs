namespace FourSolid.Cqrs.OrdiniClienti.Shared.JsonModel
{
    public class OrdineDetailsJson
    {
        public string ArticoloId { get; set; }
        public string ArticoloDescrizione { get; set; }
        public double QuantitaOrdinata { get; set; }
        public double QuantitaEvasa { get; set; }
    }
}