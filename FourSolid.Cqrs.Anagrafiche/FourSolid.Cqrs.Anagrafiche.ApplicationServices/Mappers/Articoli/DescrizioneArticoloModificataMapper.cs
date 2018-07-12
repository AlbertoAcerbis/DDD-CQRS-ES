using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using Newtonsoft.Json;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Mappers.Articoli
{
    public class DescrizioneArticoloModificataMapper : IAmAMessageMapper<DescrizioneArticoloModificata>
    {
        public Message MapToMessage(DescrizioneArticoloModificata request)
        {
            throw new System.NotImplementedException();
        }

        public DescrizioneArticoloModificata MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<DescrizioneArticoloModificata>(message.Body.Value);
        }
    }
}