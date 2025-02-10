using System;
using System.Collections.Generic;

namespace MyTools.MyEventSystem
{
    public class MyEvent
    {
        private int _eventType;
        public int EventType { get; private set; }
        internal MyEvent(int eventType)
        {
            eventDic = new Dictionary<int, List<EventHandler<EventArgs>>>();
            _eventType = eventType;
        }


        public static MyEvent CreateEvent(int eventType)
        {
            var myEvent = new MyEvent(eventType);
            EventSystem.CreateEvent((EventTypeEnum)eventType, myEvent);
            return myEvent;
        }

        private Dictionary<int, List<EventHandler<EventArgs>>> eventDic = new Dictionary<int, List<EventHandler<EventArgs>>>();

        public static MyEvent operator +(MyEvent myEvent, EventHandler<EventArgs> handler)
        {
            myEvent.AddListener(handler);
            return myEvent;
        }

        public static MyEvent operator -(MyEvent myEvent, EventHandler<EventArgs> handler)
        {
            myEvent.RemoveListener(handler);
            return myEvent;
        }

        public void AddListener(EventHandler<EventArgs> handler, int order = 0)
        {
            int key = order;
            if (eventDic.ContainsKey(key))
            {
                eventDic[key].Add(handler);
            }
            else
            {
                eventDic[key] = new List<EventHandler<EventArgs>>();
                eventDic[key].Add(handler);
            }
        }

        public void RemoveListener(EventHandler<EventArgs> handler)
        {
            foreach (var item in eventDic)
            {
                if (item.Value.Contains(handler))
                {
                    item.Value.Remove(handler);
                }
            }
        }

        public void Invoke(object sender, EventArgs e)
        {
            foreach (var item in eventDic)
            {
                foreach (var handler in item.Value)
                {
                    handler(sender, e);
                }
            }
        }

    }
}
