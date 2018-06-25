using FourSolid.Cqrs.OrdiniClienti.Domain.Rules.Resources;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.Rules
{
    public class DomainRules
    {
        public static void ChkOrdineClienteId(OrdineClienteId ordineClienteId) =>
            ordineClienteId.ChkIsValid(DomainExceptions.OrdineClienteIdNullException);

        public static void ChkClienteId(ClienteId clienteId) =>
            clienteId.ChkIsValid(DomainExceptions.ClienteIdNullException);

        public static void ChkArticoloId(ArticoloId articoloId) =>
            articoloId.ChkIsValid(DomainExceptions.ArticoloIdNullException);
    }
}