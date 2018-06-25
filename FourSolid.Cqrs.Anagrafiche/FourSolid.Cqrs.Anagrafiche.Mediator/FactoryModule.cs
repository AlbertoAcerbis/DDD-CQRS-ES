using Autofac;
using FourSolid.Cqrs.Anagrafiche.Mediator.Factory;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class FactoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServicesHandlerFactoryAsync>().As<IAmAHandlerFactoryAsync>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageMapperFactory>().As<IAmAMessageMapperFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceProviderHandlerFactory>().As<IAmAHandlerFactory>().InstancePerLifetimeScope();
        }
    }
}