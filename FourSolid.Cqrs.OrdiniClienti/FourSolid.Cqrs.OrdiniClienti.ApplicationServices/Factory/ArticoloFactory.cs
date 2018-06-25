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
    public class ArticoloFactory : IArticoloFactory
    {
        private readonly IDocumentUnitOfWork _documentUnitOfWork;
        private readonly ILogger<ArticoloFactory> _logger;

        public ArticoloFactory(IDocumentUnitOfWork documentUnitOfWork, ILogger<ArticoloFactory> logger)
        {
            this._documentUnitOfWork = documentUnitOfWork;
            this._logger = logger;
        }

        public async Task CreateArticoloAsync(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione)
        {
            try
            {
                var filter = Builders<NoSqlArticolo>.Filter.Eq("_id", articoloId.GetValue());
                var documentsResult = await this._documentUnitOfWork.NoSqlArticoloRepository.FindAsync(filter);

                if (documentsResult.Any())
                    return;

                var noSqlDocument = NoSqlArticolo.CreateNoSqlArticolo(articoloId, articoloDescrizione);
                await this._documentUnitOfWork.NoSqlArticoloRepository.InsertOneAsync(noSqlDocument);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoloFactory.CreateArticoloAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoloFactory.CreateArticoloAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }
    }
}