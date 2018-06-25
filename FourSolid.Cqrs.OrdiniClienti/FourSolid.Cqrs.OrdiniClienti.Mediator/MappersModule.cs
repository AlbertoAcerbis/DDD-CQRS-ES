using Autofac;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.Articoli;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.Clienti;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.OrdiniCliente;
using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class MappersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region OrdineCliente
            builder.RegisterType<OrdineClienteCreatedMapper>().As<IAmAMessageMapper<OrdineClienteCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region Articolo
            builder.RegisterType<ArticoloCreatedMapper>().As<IAmAMessageMapper<ArticoloCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region Cliente
            builder.RegisterType<ClienteCreatedMapper>().As<IAmAMessageMapper<ClienteCreated>>().AsSelf()
                .InstancePerLifetimeScope();
            #endregion
        }
    }
}