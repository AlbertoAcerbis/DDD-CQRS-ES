using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using Newtonsoft.Json;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.Articoli
{
    public class ArticoloCreatedMapper : IAmAMessageMapper<ArticoloCreated>
    {
        public Message MapToMessage(ArticoloCreated request)
        {
            throw new System.NotImplementedException();
        }

        public ArticoloCreated MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<ArticoloCreated>(message.Body.Value);
        }
    }
}