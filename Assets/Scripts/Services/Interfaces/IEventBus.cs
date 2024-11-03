using Services.EventBus;
using Services.ServiceLocator;
using UnityEngine.Events;

namespace Services.Interfaces
{
    public interface IEventBus : IService
    {
        public void Subscribe(EventList eventId, UnityAction action);

        public void CallEvent(EventList eventId);
    }
}