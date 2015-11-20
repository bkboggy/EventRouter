using System;

namespace EventRouter
{
    public class BroadcastToken
    {
        public string BroadcastName { get; }
        public string SourceName { get; }
        public object Source { get; }
        public EventHandler Event;

        public BroadcastToken(string broadcastName, string sourceName, object source)
        {
            BroadcastName = broadcastName;
            SourceName = sourceName;
            Source = source;
        }

        public virtual void Broadcast(object source, EventArgs args = null)
        {
            Event?.Invoke(this, args ?? new EventArgs());
        }
    }
}
