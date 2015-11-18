using System;
using System.Collections.Generic;
using System.Threading;
using Timer = System.Timers.Timer;

namespace EventRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer();
            var message = new TimerMessage(timer);

            timer.Interval = 5000;
            timer.Elapsed += message.Broadcast;
            timer.Start();

            var router = new MessageRouter();

            router.AddMessage("timerMessage", message);
            router.SubscribeByMessageType(typeof(TimerMessage), handler);

            new ManualResetEvent(false).WaitOne();
        }

        private static void handler(object sender, EventArgs e)
        {
            Console.WriteLine("Message received.");
        }
    }

    public class MessageRouter
    {
        public Dictionary<string, Message> Messages { get; private set; }

        public MessageRouter()
        {
            Messages = new Dictionary<string, Message>();
        }

        public void AddMessage(string messageName, Message message)
        {
            Messages.Add(messageName, message);
        }

        public bool SubscribeByMessageName(string messageName, EventHandler handler)
        {
            if (Messages.ContainsKey(messageName))
            {
                Messages[messageName].Event += handler;
                return true;
            }
            return false;
        }

        public int SubscribeBySource(object source, EventHandler handler)
        {
            var count = 0;
            foreach (var item in Messages)
            {
                if (item.Value.Source != source)
                {
                    continue;
                }
                SubscribeByMessageName(item.Key, handler);
                count++;
            }
            return count;
        }

        public int SubscribeBySourceType(Type sourceType, EventHandler handler)
        {
            var count = 0;
            foreach (var item in Messages)
            {
                if (item.Value.SourceType != sourceType)
                {
                    continue;
                }
                SubscribeByMessageName(item.Key, handler);
                count++;
            }
            return count;
        }

        public int SubscribeByMessageType(Type messageType, EventHandler handler)
        {
            var count = 0;
            foreach (var item in Messages)
            {
                if (item.Value.GetType() != messageType)
                {
                    continue;
                }
                SubscribeByMessageName(item.Key, handler);
                count++;
            }
            return count;
        }
    }

    public abstract class Message
    {
        public object Source { get; }
        public Type SourceType { get; }
        public EventHandler Event;

        public Message(object source)
        {
            Source = source;
            SourceType = source.GetType();
        }

        public virtual void Broadcast(EventArgs args = null)
        {
            Event?.Invoke(Source, args ?? new EventArgs());
        }

        public virtual void Broadcast(object sender, EventArgs args)
        {
            Broadcast(args);
        }
    }

    public class TimerMessage : Message
    {
        public TimerMessage(object source) : base(source) { }
    }
}
