using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.ValueObjects;
using MongoDB.Driver;

namespace FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Documents
{
    public class NoSqlArticolo : DocumentEntity
    {
        public string ArticoloDescrizione { get; private set; }
        public string UnitaMisura { get; private set; }
        public double ScortaMinima { get; private set; }

        protected NoSqlArticolo()
        { }

        #region ctor
        public static NoSqlArticolo CreateNoSqlArticolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
            ScortaMinima scortaMinima)

        {
            return new NoSqlArticolo(articoloId, articoloDescrizione, unitaMisura, scortaMinima);
        }

        private NoSqlArticolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
            ScortaMinima scortaMinima)
        {
            this.Id = articoloId.GetValue();

            this.ArticoloDescrizione = articoloDescrizione.GetValue();
            this.UnitaMisura = unitaMisura.GetValue();
            this.ScortaMinima = scortaMinima.GetValue();
        }
        #endregion

        public UpdateDefinition<NoSqlArticolo> UpdateDescrizione(ArticoloDescrizione descrizione)
        {
            return Builders<NoSqlArticolo>.Update.Set(a => a.ArticoloDescrizione, descrizione.GetValue());
        }

        #region ToJson()
        public ArticoloJson ToJson()
        {
            return new ArticoloJson
            {
                ArticoloId = this.Id,
                ArticoloDescrizione = this.ArticoloDescrizione,
                UnitaMisura = this.UnitaMisura,
                ScortaMinima = this.ScortaMinima
            };
        }
        #endregion
    }
}