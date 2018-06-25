using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using Newtonsoft.Json;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.OrdiniCliente
{
    public class OrdineClienteCreatedMapper : IAmAMessageMapper<OrdineClienteCreated>
    {
        public Message MapToMessage(OrdineClienteCreated request)
        {
            throw new System.NotImplementedException();
        }

        public OrdineClienteCreated MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<OrdineClienteCreated>(message.Body.Value);
        }
    }
}