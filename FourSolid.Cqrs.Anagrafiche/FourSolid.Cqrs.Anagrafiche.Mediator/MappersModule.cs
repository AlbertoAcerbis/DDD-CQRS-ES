using Autofac;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Mappers.Articoli;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Mappers.Clienti;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class MappersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticoloCreatedMapper>().As<IAmAMessageMapper<ArticoloCreated>>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClienteCreatedMapper>().As<IAmAMessageMapper<ClienteCreated>>().AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}