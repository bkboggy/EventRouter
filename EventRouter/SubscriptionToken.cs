using System;

namespace EventRouter
{
    public class SubscriptionToken
    {
        public string BroadcastName { get; }
        public Type BroadcastType { get; }
        public string SourceName { get; }
        public Type SourceType { get; }
        public object Subscriber { get; }
        public EventHandler Handler { get; }

        public SubscriptionToken(object subscriber, EventHandler handler, string broadcastName = null, 
            Type broadcastType = null, string sourceName = null, Type sourceType = null)
        {
            BroadcastName = broadcastName;
            BroadcastType = broadcastType;
            SourceType = sourceType;
            Handler = handler;
            Subscriber = subscriber;
        }
    }
}
