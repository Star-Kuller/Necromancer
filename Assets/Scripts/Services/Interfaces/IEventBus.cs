using Services.DependencyInjection;
using Services.EventBus;
using UnityEngine.Events;

namespace Services.Interfaces
{
    public interface IEventBus : IInjectable
    {
        public void Subscribe(GameEventList gameEventId, UnityAction<object> action);
        public void Subscribe(ViewEventList gameEventId, UnityAction<object> action);

        public void CallEvent(GameEventList gameEventId, object parameters = default);
        public void CallEvent(ViewEventList gameEventId, object parameters = default);
    }
}