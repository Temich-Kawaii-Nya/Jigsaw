using System;
using System.Collections.Generic;

public class EventBus
{
    private Dictionary<Type, List<WeakReference<IBaseEventReceiver>>> _receivers;
    private Dictionary<int, WeakReference<IBaseEventReceiver>> _referencesHash;
    
    public EventBus()
    {
        _receivers = new Dictionary<Type, List<WeakReference<IBaseEventReceiver>>>();
        _referencesHash = new Dictionary<int, WeakReference<IBaseEventReceiver>>();
    }
    public void Register<T>(IEventReceiver<T> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        
        if (!_receivers.ContainsKey(eventType))
            _receivers[eventType] = new List<WeakReference<IBaseEventReceiver>>();
        
        WeakReference<IBaseEventReceiver> reference = new(receiver);
        
        _receivers[eventType].Add(reference);
        _referencesHash[receiver.GetHashCode()] = reference;
    }
    public void Unregister<T>(IEventReceiver<T> receiver) where T : struct, IEvent     
    {
        Type eventType = typeof(T);
        int hash = receiver.GetHashCode();

        if (!(_receivers.ContainsKey(eventType) || _referencesHash.ContainsKey(hash)))
            return;

        WeakReference<IBaseEventReceiver> reference = _referencesHash[receiver.GetHashCode()];
        _receivers[eventType].Remove(reference);
        _referencesHash.Remove(hash);
    }
    public void TriggerEvent<T>(T eventMessage) where T : struct, IEvent
    {
        Type eventType = typeof(T);

        if (!_receivers.ContainsKey(eventType))
            return;

        foreach (var reference in _receivers[eventType])
        {
            if (reference.TryGetTarget(out IBaseEventReceiver receiver))
            {
                ((IEventReceiver<T>)receiver).OnEvent(eventMessage);
            }
        }
    }
}
