using Autofac;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Articoli;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Clienti;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class EventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticoloCreatedEventHandler>().As<IHandleRequests<ArticoloCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<DescrizioneArticoloModificataEventHandler>()
                .As<IHandleRequests<DescrizioneArticoloModificata>>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<ClienteCreatedEventHandler>().As<IHandleRequests<ClienteCreated>>().AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}