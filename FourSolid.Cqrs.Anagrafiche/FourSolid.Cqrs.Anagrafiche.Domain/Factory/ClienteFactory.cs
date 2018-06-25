using FourSolid.Cqrs.Anagrafiche.Domain.Entities;
using FourSolid.Cqrs.Anagrafiche.Domain.Rules;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Factory
{
    public class ClienteFactory
    {
        internal static Cliente CreateCliente(ClienteId clienteId, RagioneSociale ragioneSociale,
            CodiceFiscale codiceFiscale, PartitaIva partitaIva, AccountInfo who, When when)
        {
            DomainRules.ChkClienteId(clienteId);
            DomainRules.ChkRagioneSociale(ragioneSociale);
            DomainRules.ChkPartitaIva(partitaIva);
            DomainRules.ChkCodiceFiscale(codiceFiscale);

            return new Cliente(clienteId, ragioneSociale, codiceFiscale, partitaIva, who, when);
        }
    }
}