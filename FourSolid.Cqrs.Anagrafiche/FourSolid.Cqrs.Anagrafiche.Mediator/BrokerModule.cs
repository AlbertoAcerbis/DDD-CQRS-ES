using Autofac;
using FourSolid.MessagingGateway.RabbitMq.Abstracts;
using FourSolid.MessagingGateway.RabbitMq.Concretes;
using FourSolid.MessagingGateway.RabbitMq.Configuration;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class BrokerModule : Module
    {
        private readonly RmqParameters _rmqParameters;

        public BrokerModule(RmqParameters rmqParameters)
        {
            this._rmqParameters = rmqParameters;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var brokerClient = new BrokerClient(this._rmqParameters);
            builder.RegisterInstance(brokerClient).As<IBrokerClient>().SingleInstance();
        }
    }
}