using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Shared.ValueObjects;
using MongoDB.Driver;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Documents
{
    public class NoSqlArticolo : DocumentEntity
    {
        public string ArticoloDescrizione { get; private set; }

        protected NoSqlArticolo()
        { }

        #region ctor
        public static NoSqlArticolo CreateNoSqlArticolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione)

        {
            return new NoSqlArticolo(articoloId, articoloDescrizione);
        }

        private NoSqlArticolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione)
        {
            this.Id = articoloId.GetValue();
            this.ArticoloDescrizione = articoloDescrizione.GetValue();
        }
        #endregion

        public UpdateDefinition<NoSqlArticolo> UpdateDescrizione(ArticoloDescrizione descrizione)
        {
            return Builders<NoSqlArticolo>.Update.Set(a => a.ArticoloDescrizione, descrizione.GetValue());
        }
    }
}