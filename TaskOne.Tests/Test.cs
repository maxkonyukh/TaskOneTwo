using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskOne.Services;
using Xunit;

namespace TaskOne.Tests
{
    public class Test
    {
        // this test is to make sure that even asynchronous actions will be executated in the correct order
        [Fact]
        public async Task SequentialExecutingTest()
        {
            List<int> list = new List<int>();
            Scheduler scheduler = new Scheduler();

            scheduler.Schedule(async () =>
            {
                // longer action to simulate some IO or networking operation (the result would otherwise be populted as second item of the list)
                await Task.Delay(TimeSpan.FromSeconds(3));
                list.Add(10);
            });

            scheduler.Schedule(async () =>
            {
                // thanks to blocking collection this will be second element of the list even though this method is executing notably faster
                await Task.Delay(TimeSpan.FromSeconds(0));
                list.Add(20);
            });

            await Task.Delay(TimeSpan.FromSeconds(5));
            Assert.Equal(20, list[1]);
        }
    }
}
