using System;
using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine.Events;

namespace Services.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<GameEvent, UnityEvent<object>> _gameEvents = new();
        private readonly Dictionary<ViewEvent, UnityEvent<object>> _viewEvents = new();

        public EventBus()
        {
            foreach (GameEvent eventType in Enum.GetValues(typeof(GameEvent)))
            {
                _gameEvents[eventType] = new UnityEvent<object>();
            }
            foreach (ViewEvent eventType in Enum.GetValues(typeof(ViewEvent)))
            {
                _viewEvents[eventType] = new UnityEvent<object>();
            }
        }
        
        public void Subscribe(GameEvent gameEventId, UnityAction<object> action)
        {
            _gameEvents[gameEventId].AddListener(action);
        }

        public void Subscribe(ViewEvent gameEventId, UnityAction<object> action)
        {
            _viewEvents[gameEventId].AddListener(action);
        }

        public void CallEvent(GameEvent gameEventId, object parameters = null)
        {
            _gameEvents[gameEventId].Invoke(parameters);
        }

        public void CallEvent(ViewEvent gameEventId, object parameters = null)
        {
            _viewEvents[gameEventId].Invoke(parameters);
        }
    }
}