using System;
using System.IO;
using System.Threading.Tasks;
using TaskOne.Services;

namespace TaskOne
{
    class Program
    {
        static void Main()
        {
            Scheduler scheduler = new Scheduler();

            while (true)
            {
                string input = Console.ReadLine();

                Guid guid = Guid.NewGuid();
                scheduler.Schedule(async () =>
                {
                    // some delay to temporarily make thread doing nothing
                    // during this time we can trigger a lot of actions and observe how they are sequantially being writing to files
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    await File.WriteAllTextAsync($"file_{guid}", input);
                });
            }
        }
    }
}