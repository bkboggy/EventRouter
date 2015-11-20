using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace EventRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            var router = new Router();
            var broadcast = new BroadcastToken("TimerBroadcast", null, null);
            router.AddBroadcast(new BroadcastToken("TimerBroadcast", null, null));
            var subscription = new SubscriptionToken(null, handler, broadcastType: typeof(BroadcastToken));
            router.Subscribe(subscription);

            Timer timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += broadcast.Broadcast;
            timer.Start();

            new ManualResetEvent(false).WaitOne();
        }

        private static void handler(object sender, EventArgs e)
        {
            Console.WriteLine("Message received.");
        }
    }
}
