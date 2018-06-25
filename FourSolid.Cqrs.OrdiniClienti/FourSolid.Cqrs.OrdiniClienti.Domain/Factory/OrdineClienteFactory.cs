using FourSolid.Cqrs.OrdiniClienti.Domain.Entities;
using FourSolid.Cqrs.OrdiniClienti.Domain.Rules;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.Factory
{
    public class OrdineClienteFactory
    {
        internal static OrdineClienteMaster CreateOrdineClienteMaster(OrdineClienteId ordineClienteId,
            ClienteId clienteId, DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna,
            AccountInfo who, When when)
        {
            DomainRules.ChkOrdineClienteId(ordineClienteId);
            DomainRules.ChkClienteId(clienteId);

            return new OrdineClienteMaster(ordineClienteId, clienteId, dataInserimento, dataPrevistaConsegna, who, when);
        }
    }
}