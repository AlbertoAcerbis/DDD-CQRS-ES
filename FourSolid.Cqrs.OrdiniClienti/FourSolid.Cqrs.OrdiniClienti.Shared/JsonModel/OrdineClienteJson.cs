using System;
using System.Collections.Generic;
using System.Linq;

namespace FourSolid.Cqrs.OrdiniClienti.Shared.JsonModel
{
    public class OrdineClienteJson
    {
        public string ClienteId { get; set; }
        public string RagioneSociale { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime DataPrevistaConsegna { get; set; }

        public IEnumerable<OrdineDetailsJson> OrdineDetails { get; set; }

        public OrdineClienteJson()
        {
            this.OrdineDetails = Enumerable.Empty<OrdineDetailsJson>();
        }
    }
}