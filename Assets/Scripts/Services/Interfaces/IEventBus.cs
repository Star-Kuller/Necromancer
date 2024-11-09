using Services.DependencyInjection;
using Services.EventBus;
using UnityEngine.Events;

namespace Services.Interfaces
{
    public interface IEventBus : IInjectable
    {
        public void Subscribe(EventList eventId, UnityAction<object> action);

        public void CallEvent(EventList eventId, object parameters = default);
    }
}