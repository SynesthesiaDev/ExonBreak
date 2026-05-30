// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using System.Runtime.InteropServices;

namespace ExonBreak.Common.Dispatchers;

public class EventDispatcher<T>
{
    private readonly List<EventSubscriber<T>> eventSubscribers = [];

    public bool IsDisposed { get; set; }

    public EventSubscriber<T> Subscribe(Action<T> action)
    {
        var eventSubscriber = new EventSubscriber<T>(action);
        eventSubscribers.Add(eventSubscriber);
        return eventSubscriber;
    }

    public void Dispatch(T value)
    {
        foreach (ref EventSubscriber<T> subscriber in CollectionsMarshal.AsSpan(eventSubscribers))
        {
            subscriber.Action.Invoke(value);
        }
    }

    public void Unsubscribe(IEventSubscriber subscriber)
    {
        eventSubscribers.Remove((subscriber as EventSubscriber<T>)!);
    }

    public void UnsubscribeAll()
    {
        eventSubscribers.Clear();
    }

    public void Dispose()
    {
        if(IsDisposed) return;
        UnsubscribeAll();
        IsDisposed = true;
    }

    public void Reset()
    {
        UnsubscribeAll();
    }
}

public record EventSubscriber<T>(Action<T> Action) : IEventSubscriber;

public interface IEventSubscriber;
