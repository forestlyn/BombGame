using System;
using System.Collections.Generic;

namespace MyTools.MyEventSystem
{
    public static class EventSystem
    {
        private static Dictionary<EventTypeEnum, MyEvent> eventsDic = new Dictionary<EventTypeEnum, MyEvent>();

        public static void AddListener(EventTypeEnum eventType, EventHandler<EventArgs> handler, int order = 0)
        {
            if (eventsDic.ContainsKey(eventType) && handler != null)
            {
                eventsDic[eventType].AddListener(handler, order);
            }
            else
            {
                CreateEvent(eventType, new MyEvent((int)eventType));
                if (handler != null)
                    eventsDic[eventType].AddListener(handler, order);
            }
        }

        public static void RemoveListener(EventTypeEnum eventType, EventHandler<EventArgs> handler)
        {
            if (eventsDic.ContainsKey(eventType))
            {
                eventsDic[eventType].RemoveListener(handler);
            }
        }

        public static void Invoke(EventTypeEnum eventType, object sender, EventArgs e)
        {
            if (eventsDic.ContainsKey(eventType))
            {
                eventsDic[eventType].Invoke(sender, e);
            }
            else
            {
                MyLog.Log("EventSystem Invoke :Event not found");
            }
        }

        internal static void CreateEvent(EventTypeEnum eventType, MyEvent myEvent)
        {
            if (eventsDic.ContainsKey(eventType))
                MyLog.Log("EventSystem CreateEvent :Event already exists");
            else
                eventsDic.Add(eventType, myEvent);
        }
    }
}

