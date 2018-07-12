using Autofac;
using FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Articoli;
using FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Clienti;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateArticoloCommandHandler>().As<IHandleRequestsAsync<CreateArticolo>>().AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<ModificaDescrizioneArticoloCommandHandler>()
                .As<IHandleRequestsAsync<ModificaDescrizioneArticolo>>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<CreateClienteCommandHandler>().As<IHandleRequestsAsync<CreateCliente>>().AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}