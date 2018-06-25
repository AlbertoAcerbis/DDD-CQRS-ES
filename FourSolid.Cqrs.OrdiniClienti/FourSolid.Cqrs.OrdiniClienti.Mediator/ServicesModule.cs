using Autofac;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Factory;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Orchestrator;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrdineClienteOrchestrator>().As<IOrdineClienteOrchestrator>()
                            .InstancePerLifetimeScope();
            builder.RegisterType<OrdineClienteFactory>().As<IOrdineClienteFactory>().InstancePerLifetimeScope();

            builder.RegisterType<ArticoloFactory>().As<IArticoloFactory>().InstancePerLifetimeScope();

            builder.RegisterType<ClienteFactory>().As<IClienteFactory>().InstancePerLifetimeScope();
        }
    }
}