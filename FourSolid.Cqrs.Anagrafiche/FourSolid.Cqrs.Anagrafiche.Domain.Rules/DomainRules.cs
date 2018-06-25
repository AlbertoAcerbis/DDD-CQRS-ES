using FourSolid.Cqrs.Anagrafiche.Domain.Rules.Resources;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Rules
{
    public class DomainRules
    {
        #region Articolo
        public static void ChkArticoloId(ArticoloId articoloId) =>
            articoloId.ChkIsValid(DomainExceptions.ArticoloIdNullException);

        public static void ChkArticoloDescrizione(ArticoloDescrizione articoloDescrizione) =>
            articoloDescrizione.ChkIsValid(DomainExceptions.ArticoloDescrizioneNullException);

        public static void ChkUnitaMisura(UnitaMisura unitaMisura) =>
            unitaMisura.ChkIsValid(DomainExceptions.UnitaMisuraNullException);
        #endregion

        #region Cliente
        public static void ChkClienteId(ClienteId clienteId) =>
            clienteId.ChkIsValid(DomainExceptions.ClienteIdNullException);

        public static void ChkRagioneSociale(RagioneSociale ragioneSociale) =>
            ragioneSociale.ChkIsValid(DomainExceptions.RagioneSocialeNullException);

        public static void ChkCodiceFiscale(CodiceFiscale codiceFiscale) =>
            codiceFiscale.ChkIsValid(DomainExceptions.CodiceFiscaleNullException);

        public static void ChkPartitaIva(PartitaIva partitaIva) =>
            partitaIva.ChkIsValid(DomainExceptions.PartitaIvaNullException);
        #endregion
    }
}