using Autofac;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Factory;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Orchestrator;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticoloOrchestrator>().As<IArticoloOrchestrator>().InstancePerLifetimeScope();
            builder.RegisterType<ArticoloFactory>().As<IArticoloFactory>().InstancePerLifetimeScope();

            builder.RegisterType<ClienteOrchestrator>().As<IClienteOrchestrator>().InstancePerLifetimeScope();
            builder.RegisterType<ClienteFactory>().As<IClienteFactory>().InstancePerLifetimeScope();
        }
    }
}