using System;
using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine.Events;

namespace Services.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<GameEventList, UnityEvent<object>> _gameEvents = new();
        private readonly Dictionary<ViewEventList, UnityEvent<object>> _viewEvents = new();

        public EventBus()
        {
            foreach (GameEventList eventType in Enum.GetValues(typeof(GameEventList)))
            {
                _gameEvents[eventType] = new UnityEvent<object>();
            }
            foreach (ViewEventList eventType in Enum.GetValues(typeof(ViewEventList)))
            {
                _viewEvents[eventType] = new UnityEvent<object>();
            }
        }
        
        public void Subscribe(GameEventList gameEventId, UnityAction<object> action)
        {
            _gameEvents[gameEventId].AddListener(action);
        }

        public void Subscribe(ViewEventList gameEventId, UnityAction<object> action)
        {
            _viewEvents[gameEventId].AddListener(action);
        }

        public void CallEvent(GameEventList gameEventId, object parameters = default)
        {
            _gameEvents[gameEventId].Invoke(parameters);
        }

        public void CallEvent(ViewEventList gameEventId, object parameters = default)
        {
            _viewEvents[gameEventId].Invoke(parameters);
        }
    }
}