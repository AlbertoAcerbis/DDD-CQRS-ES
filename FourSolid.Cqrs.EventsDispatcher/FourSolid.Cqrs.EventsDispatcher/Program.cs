using Autofac;
using FourSolid.Cqrs.EventsDispatcher.EventStore;

namespace FourSolid.Cqrs.EventsDispatcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = AutofacBootstrapper.RegisterModules();
            var container = builder.Build();

            var eventDispatcher = container.Resolve<EventDispatcher>();
            eventDispatcher.StartDispatching();
        }
    }
}
