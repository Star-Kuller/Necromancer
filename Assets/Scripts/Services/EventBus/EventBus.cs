using System;
using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine.Events;

namespace Services.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<EventList, UnityEvent<object>> _events = new();

        public EventBus()
        {
            foreach (EventList eventType in Enum.GetValues(typeof(EventList)))
            {
                _events[eventType] = new UnityEvent<object>();
            }
        }
        
        public void Subscribe(EventList eventId, UnityAction<object> action)
        {
            _events[eventId].AddListener(action);
        }

        public void CallEvent(EventList eventId, object parameters = default)
        {
            _events[eventId].Invoke(parameters);
        }
    }
}