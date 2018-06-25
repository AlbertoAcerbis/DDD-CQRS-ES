using Autofac;
using FourSolid.Cqrs.OrdiniClienti.Domain.CommandsHandler;
using FourSolid.Cqrs.OrdiniClienti.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateOrdineClienteCommandHandler>().As<IHandleRequestsAsync<CreateOrdineCliente>>().AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}