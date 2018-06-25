using System;
using System.Linq;
using System.Threading.Tasks;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Documents;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;
using FourSolid.Shared.Services;
using FourSolid.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Factory
{
    public class OrdineClienteFactory : IOrdineClienteFactory
    {
        private readonly IDocumentUnitOfWork _documentUnitOfWork;
        private readonly ILogger<OrdineClienteFactory> _logger;

        public OrdineClienteFactory(IDocumentUnitOfWork documentUnitOfWork, ILogger<OrdineClienteFactory> logger)
        {
            this._documentUnitOfWork = documentUnitOfWork;
            this._logger = logger;
        }

        public async Task CreateOrdineClienteAsync(OrdineClienteId ordineClienteId, ClienteId clienteId, RagioneSociale ragioneSociale,
            DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna)
        {
            try
            {
                var filter = Builders<NoSqlOrdineCliente>.Filter.Eq("_id", ordineClienteId.GetValue());
                var documentsResult = await this._documentUnitOfWork.NoSqlOrdineClienteRepository.FindAsync(filter);

                if (!documentsResult.Any())
                    return;

                var noSqlDocument = NoSqlOrdineCliente.CreateNoSqlOrdineCliente(ordineClienteId, clienteId,
                    ragioneSociale, dataInserimento, dataPrevistaConsegna);
                await this._documentUnitOfWork.NoSqlOrdineClienteRepository.InsertOneAsync(noSqlDocument);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[OrdineClienteFactory.CreateOrdineClienteAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[OrdineClienteFactory.CreateOrdineClienteAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }
    }
}