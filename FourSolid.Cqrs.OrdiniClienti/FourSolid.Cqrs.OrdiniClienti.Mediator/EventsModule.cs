using Autofac;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Articoli;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Clienti;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.OrdiniCliente;
using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class EventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region OrdineCliente
            builder.RegisterType<OrdineClienteCreatedEventHandler>().As<IHandleRequests<OrdineClienteCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region Articolo
            builder.RegisterType<ArticoloCreatedEventHandler>().As<IHandleRequests<ArticoloCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<DescrizioneArticoloModificataEventHandler>()
                .As<IHandleRequests<DescrizioneArticoloModificata>>().AsSelf().InstancePerLifetimeScope();
            #endregion

            #region Cliente
            builder.RegisterType<ClienteCreatedEventHandler>().As<IHandleRequests<ClienteCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            #endregion
        }
    }
}