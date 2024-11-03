using System;
using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine.Events;

namespace Services.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<EventList, UnityEvent> _events = new();

        public EventBus()
        {
            foreach (EventList eventType in Enum.GetValues(typeof(EventList)))
            {
                _events[eventType] = new UnityEvent();
            }
        }
        
        public void Subscribe(EventList eventId, UnityAction action)
        {
            _events[eventId].AddListener(action);
        }

        public void CallEvent(EventList eventId)
        {
            _events[eventId].Invoke();
        }
    }
}