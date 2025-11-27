using System.Collections.Generic;
using System.Diagnostics;

public static class EventManager
{
    public delegate void EventDelegate(params object[] parameters);
    static Dictionary<EventType, EventDelegate> _events = new Dictionary<EventType, EventDelegate>();

    public static void Subscribe(EventType type, EventDelegate method)
    {
        if (_events.ContainsKey(type))
            _events[type] += method;
        else
            _events.Add(type, method);
    }

    public static void Unsubscribe(EventType type, EventDelegate method)
    {
        if (_events.ContainsKey(type))
        {
            _events[type] -= method;

            if (_events[type] == null)
                _events.Remove(type);
        }
    }

    public static void Trigger(EventType type, params object[] parameters)
    {
        //Debug.WriteLineIf(EventType.OnAdFinished == type, "Event Triggered: " + type.ToString());
        if (_events.ContainsKey(type))
            _events[type](parameters);

    }
}
