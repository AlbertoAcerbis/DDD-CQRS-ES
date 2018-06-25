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
    public class ArticoloFactory : IArticoloFactory
    {
        private readonly IDocumentUnitOfWork _documentUnitOfWork;
        private readonly ILogger<ArticoloFactory> _logger;

        public ArticoloFactory(IDocumentUnitOfWork documentUnitOfWork, ILogger<ArticoloFactory> logger)
        {
            this._documentUnitOfWork = documentUnitOfWork;
            this._logger = logger;
        }

        public async Task CreateArticoloAsync(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
            ScortaMinima scortaMinima)
        {
            try
            {
                var filter = Builders<NoSqlArticolo>.Filter.Eq("_id", articoloId.GetValue());
                var documentsResult = await this._documentUnitOfWork.NoSqlArticoloRepository.FindAsync(filter);

                if (documentsResult.Any())
                    return;

                var noSqlDocument =
                    NoSqlArticolo.CreateNoSqlArticolo(articoloId, articoloDescrizione, unitaMisura, scortaMinima);
                await this._documentUnitOfWork.NoSqlArticoloRepository.InsertOneAsync(noSqlDocument);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoloFactory.CreateArticoloAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoloFactory.CreateArticoloAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        public async Task<IEnumerable<ArticoloJson>> GetArticoliAsync()
        {
            try
            {
                var filter = Builders<NoSqlArticolo>.Filter.Empty;
                var documentsResult = await this._documentUnitOfWork.NoSqlArticoloRepository.FindAsync(filter);

                return documentsResult.Any()
                    ? documentsResult.Select(noSqlDocument => noSqlDocument.ToJson())
                    : Enumerable.Empty<ArticoloJson>();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoloFactory.GetArticoliAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoloFactory.GetArticoliAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        public async Task<ArticoloJson> GetArticoloByIdAsync(ArticoloId articoloId)
        {
            try
            {
                var filter = Builders<NoSqlArticolo>.Filter.Eq("_id", articoloId.GetValue());
                var documentsResult = await this._documentUnitOfWork.NoSqlArticoloRepository.FindAsync(filter);

                return documentsResult.Any()
                    ? documentsResult.First().ToJson()
                    : new ArticoloJson();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoloFactory.GetArticoloByIdAsync] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoloFactory.GetArticoloByIdAsync] - {CommonServices.GetErrorMessage(ex)}");
            }
        }
    }
}