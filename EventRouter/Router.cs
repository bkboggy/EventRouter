using System;
using System.Collections.Generic;
using System.Linq;

namespace EventRouter
{
    public class Router
    {
        public List<BroadcastToken> Broadcasts { get; }
        public List<SubscriptionToken> Subscribtions { get; }

        public Router()
        {
            Broadcasts = new List<BroadcastToken>();
            Subscribtions = new List<SubscriptionToken>();
        }

        public bool AddBroadcast(BroadcastToken broadcastToken)
        {
            if (broadcastToken == null)
            {
                throw new ArgumentNullException(nameof(broadcastToken));
            }
            if (!Broadcasts.Contains(broadcastToken))
            {
                broadcastToken.Event += Broadcast;
                Broadcasts.Add(broadcastToken);
                return true;
            }
            return false;
        }

        public bool RemoveBroadcast(BroadcastToken broadcastToken)
        {
            if (broadcastToken == null)
            {
                throw new ArgumentNullException(nameof(broadcastToken));
            }
            if (Broadcasts.Contains(broadcastToken))
            {
                Broadcasts.Remove(broadcastToken);
                return true;
            }
            return false;
        }

        public bool RemoveBroadcast(string broadcastName)
        {
            if (broadcastName == null)
            {
                throw new ArgumentNullException(nameof(broadcastName));
            }
            else if (string.IsNullOrWhiteSpace(broadcastName))
            {
                throw new ArgumentException("Argument may not be empty or contain only white space.", nameof(broadcastName));
            }
            var broadcastToken = Broadcasts.FirstOrDefault(x => x.BroadcastName.Equals(broadcastName));
            if (broadcastToken != null)
            {
                return RemoveBroadcast(broadcastToken);
            }
            return false;
        }

        public bool Subscribe(SubscriptionToken subscriptionToken)
        {
            if (subscriptionToken == null)
            {
                throw new ArgumentNullException(nameof(subscriptionToken));
            }
            if (!Subscribtions.Contains(subscriptionToken))
            {
                Subscribtions.Add(subscriptionToken);
                return true;
            }
            return false;
        }

        public bool Unsubscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            var subscriptionToken = Subscribtions.FirstOrDefault(s => s.Subscriber == subscriber);
            if (subscriptionToken != null)
            {
                Subscribtions.Remove(subscriptionToken);
                return true;
            }
            return false;
        }

        private void Broadcast(object sender, EventArgs e)
        {
            var broadcastToken = (BroadcastToken)sender;
            foreach (var subscriber in Subscribtions)
            {
                var notify = false;
                if (!string.IsNullOrWhiteSpace(subscriber.BroadcastName))
                {
                    if (subscriber.BroadcastName.Equals(broadcastToken.BroadcastName))
                    {
                        notify = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (subscriber.BroadcastType != null)
                {
                    if (subscriber.BroadcastType == broadcastToken.GetType())
                    {
                        notify = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrWhiteSpace(subscriber.SourceName))
                {
                    if (subscriber.SourceName.Equals(broadcastToken.SourceName))
                    {
                        notify = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (subscriber.SourceType != null)
                {
                    if (subscriber.SourceType == broadcastToken.Source?.GetType())
                    {
                        notify = true;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (notify)
                {
                    subscriber.Handler?.Invoke(broadcastToken.Source, e);
                }
            }
        }
    }
}
