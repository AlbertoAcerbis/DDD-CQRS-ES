using FourSolid.Common.InProcessBus.Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paramore.Brighter;

namespace FourSolid.Common.InProcessBus
{
    public class ServiceBus : IServiceBus, IEventBus, IDisposable
    {
        private readonly Dictionary<Type, List<Action<Event>>> _routes = new Dictionary<Type, List<Action<Event>>>();
        private readonly IAmACommandProcessor _commandProcessor;

        public ServiceBus(IAmACommandProcessor commandProcessor)
        {
            this._commandProcessor = commandProcessor;
        }

        #region IServiceBus
        public async Task SendAsync<T>(T command) where T : Command
        {
            await this._commandProcessor.SendAsync(command);
        }

        public void RegisterHandler<T>(Action<T> handler) where T : Event
        {
            if (!this._routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Action<Event>>();
                this._routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<Event, T>(x => handler(x)));
        }
        #endregion

        #region IEventBus
        public Task PublishAsync(Event @event)
        {
            //if (!this._routes.TryGetValue(@event.GetType(), out var handlers)) return Task.CompletedTask;

            //var eventArray = @event.GetType().ToString().Split('.');
            //var routingKey = eventArray[eventArray.Length - 1];
            //this._brokerClient.Send(@event, routingKey);

            return Task.CompletedTask;

            //foreach (var handler in handlers)
            //{
            //    //this._logService.LoggerTrace($"Evento {@event.GetType()}; AggregateId {@event.AggregateId}");
            //    //handler(@event);
            //}
        }
        #endregion

        #region Dispose
        private bool _disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // Qui si devono liberare le risorse eventualmente allocate da questo oggetto
                    // In questo caso ... nothing to do!!!
                }
            }
            this._disposed = true;
        }
        #endregion
    }
}