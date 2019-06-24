using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TaskOne.Services
{

    public class Scheduler
    {
        // producer consumer came to my mind so I used this blocking collection which is basically implementation of this pattern
        private readonly BlockingCollection<Func<Task>> actions = new BlockingCollection<Func<Task>>(new ConcurrentQueue<Func<Task>>());

        public Scheduler()
        {
            Consume();
        }

        public void Schedule(Func<Task> action)
        {
            actions.Add(action);
        }

        private void Consume()
        {
            // old school way to create a thread. We actually have more control by using low level entities then let's say Task.Run
            // since we probably do not want any ThreadPool here (for the sake of testing task)
            Thread thread = new Thread(async () =>
            {
                while (!actions.IsCompleted)
                {
                    try
                    {
                        Func<Task> action = actions.Take();
                        await action();
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Adding was completed!");
                        break;
                    }
                }
            });

            thread.Start();
        }
    }
}
