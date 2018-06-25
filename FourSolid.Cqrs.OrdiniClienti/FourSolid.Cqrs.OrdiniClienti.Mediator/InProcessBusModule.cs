using Autofac;
using FourSolid.Common.InProcessBus;
using FourSolid.Common.InProcessBus.Abstracts;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class InProcessBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceBus>().As<IServiceBus>().As<IEventBus>().SingleInstance();
        }
    }
}