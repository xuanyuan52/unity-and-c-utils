using System;
using System.Collections.Generic;
using System.Threading;

public sealed class EventSet {
    
    private readonly Dictionary<string, Delegate> m_events = new Dictionary<string, Delegate>();

    public void Add(string key, Delegate handler)
    {
        Monitor.Enter(m_events);
        Delegate d;
        m_events.TryGetValue(key, out d);
        m_events[key] = Delegate.Combine(d, handler);
        Monitor.Exit(m_events);
    }

    public void Remove(string key, Delegate handler)
    {
        Monitor.Enter(m_events);
        Delegate d;
        if (m_events.TryGetValue(key, out d))
        {
            d = Delegate.Remove(d, handler);
            if (d != null) m_events[key] = d;
            else m_events.Remove(key);
        }
        Monitor.Exit(m_events);
    }

    public void Broadcast(string key, object data)
    {
        Delegate d;
        Monitor.Enter(m_events);
        m_events.TryGetValue(key, out d);
        Monitor.Exit(m_events);
        if (d != null)
        {
            d.DynamicInvoke(data);
        }
    }

    public void Broadcast(string key, object sender, object data)
    {
        Delegate d;
        Monitor.Enter(m_events);
        m_events.TryGetValue(key, out d);
        Monitor.Exit(m_events);
        if (d != null)
        {
            d.DynamicInvoke(new object[] {sender, data});
        }
    }

    public void Broadcast(string key, object sender, EventArgs e)
    {
        Delegate d;
        Monitor.Enter(m_events);
        m_events.TryGetValue(key, out d);
        Monitor.Exit(m_events);
        if (d != null)
        {
            d.DynamicInvoke(new object[] { sender, e });
        }
    }
}
