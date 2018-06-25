using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.OrdiniCliente
{
    public class OrdineClienteCreatedEventHandler : RequestHandler<OrdineClienteCreated>
    {
        public override OrdineClienteCreated Handle(OrdineClienteCreated command)
        {
            return base.Handle(command);
        }
    }
}