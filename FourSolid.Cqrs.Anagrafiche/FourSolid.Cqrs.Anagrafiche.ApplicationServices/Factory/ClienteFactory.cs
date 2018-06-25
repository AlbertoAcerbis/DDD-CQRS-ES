using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Documents;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.Services;
using FourSolid.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Factory
{
    public class ClienteFactory : IClienteFactory
    {
        private readonly IDocumentUnitOfWork _documentUnitOfWork;
        private readonly ILogger<ClienteFactory> _logger;

        public ClienteFactory(IDocumentUnitOfWork documentUnitOfWork, ILogger<ClienteFactory> logger)
        {
            this._documentUnitOfWork = documentUnitOfWork;
            this._logger = logger;
        }

        public async Task CreateClienteAsync(ClienteId clienteId, RagioneSociale ragioneSociale, CodiceFiscale codiceFiscale,
            PartitaIva partitaIva)
        {
            try
            {
                var filter = Builders<NoSqlCliente>.Filter.Eq("_id", clienteId.GetValue());
                var documentResults = await this._documentUnitOfWork.NoSqlClienteRepository.FindAsync(filter);

                if (documentResults.Any())
                    return;

                var noSqlDocument =
                    NoSqlCliente.CreateNoSqlCliente(clienteId, ragioneSociale, partitaIva, codiceFiscale);
                await this._documentUnitOfWork.NoSqlClienteRepository.InsertOneAsync(noSqlDocument);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClienteFactory.CreateClienteAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ClienteFactory.CreateClienteAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        public async Task<IEnumerable<ClienteJson>> GetClientiAsync()
        {
            try
            {
                var filter = Builders<NoSqlCliente>.Filter.Empty;
                var documentResults = await this._documentUnitOfWork.NoSqlClienteRepository.FindAsync(filter);

                return documentResults.Any()
                    ? documentResults.Select(noSqlDocument => noSqlDocument.ToJson())
                    : Enumerable.Empty<ClienteJson>();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClienteFactory.GetClientiAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ClienteFactory.GetClientiAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        public async Task<ClienteJson> GetClienteDetailsByIdAsync(ClienteId clienteId)
        {
            try
            {
                var filter = Builders<NoSqlCliente>.Filter.Eq("_id", clienteId.GetValue());
                var documentResults = await this._documentUnitOfWork.NoSqlClienteRepository.FindAsync(filter);

                return documentResults.Any()
                    ? documentResults.First().ToJson()
                    : new ClienteJson();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClienteFactory.GetClienteDetailsByIdAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ClienteFactory.GetClienteDetailsByIdAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }
    }
}