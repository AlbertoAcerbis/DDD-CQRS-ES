using System.Threading.Tasks;
using Paramore.Brighter;

namespace FourSolid.Common.InProcessBus.Abstracts
{
    public interface IEventBus
    {
        Task PublishAsync(Event @event);
    }
}