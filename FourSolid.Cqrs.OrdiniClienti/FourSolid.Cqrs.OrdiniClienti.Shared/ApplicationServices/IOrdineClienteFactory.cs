using System.Threading.Tasks;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices
{
    public interface IOrdineClienteFactory
    {
        Task CreateOrdineClienteAsync(OrdineClienteId ordineClienteId, ClienteId clienteId,
            RagioneSociale ragioneSociale, DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna);
    }
}