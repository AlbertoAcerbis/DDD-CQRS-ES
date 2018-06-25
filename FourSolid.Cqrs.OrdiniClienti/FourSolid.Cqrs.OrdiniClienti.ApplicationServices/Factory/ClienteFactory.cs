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
    public class ClienteFactory : IClienteFactory
    {
        private readonly IDocumentUnitOfWork _documentUnitOfWork;
        private readonly ILogger<ClienteFactory> _logger;

        public ClienteFactory(IDocumentUnitOfWork documentUnitOfWork, 
            ILogger<ClienteFactory> logger)
        {
            this._documentUnitOfWork = documentUnitOfWork;
            this._logger = logger;
        }

        public async Task CreateClienteAsync(ClienteId clienteId, RagioneSociale ragioneSociale)
        {
            try
            {
                var filter = Builders<NoSqlCliente>.Filter.Eq("_id", clienteId.GetValue());
                var documentsResult = await this._documentUnitOfWork.NoSqlClienteRepository.FindAsync(filter);

                if (documentsResult.Any())
                    return;

                var noSqlDocument = NoSqlCliente.CreateNoSqlCliente(clienteId, ragioneSociale);
                await this._documentUnitOfWork.NoSqlClienteRepository.InsertOneAsync(noSqlDocument);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClienteFactory.CreateClienteAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ClienteFactory.CreateClienteAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }
    }
}