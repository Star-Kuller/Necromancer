using Services.DependencyInjection;
using Services.EventBus;
using UnityEngine.Events;

namespace Services.Interfaces
{
    public interface IEventBus : IInjectable
    {
        public void Subscribe(GameEvent gameEventId, UnityAction<object> action);
        public void Subscribe(ViewEvent gameEventId, UnityAction<object> action);

        public void CallEvent(GameEvent gameEventId, object parameters = default);
        public void CallEvent(ViewEvent gameEventId, object parameters = default);
    }
}