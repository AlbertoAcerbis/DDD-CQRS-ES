using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using Newtonsoft.Json;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.Clienti
{
    public class ClienteCreatedMapper : IAmAMessageMapper<ClienteCreated>
    {
        public Message MapToMessage(ClienteCreated request)
        {
            throw new System.NotImplementedException();
        }

        public ClienteCreated MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<ClienteCreated>(message.Body.Value);
        }
    }
}