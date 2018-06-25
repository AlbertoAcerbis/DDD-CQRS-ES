using System;
using System.Threading.Tasks;
using Paramore.Brighter;

namespace FourSolid.Common.InProcessBus.Abstracts
{
    public interface IServiceBus
    {
        Task SendAsync<T>(T command) where T : Command;

        void RegisterHandler<T>(Action<T> handler) where T : Event;
    }
}