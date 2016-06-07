using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncHelpers.Tests
{
    internal static class TestTasks
    {
        internal async static Task<string> GetResult(TimeSpan ts, int throwEx = 0)
        {
            if (throwEx == 1)
                throw new ArgumentException("Boom before delay!!!");

            await Task.Delay(ts);

            if (throwEx == 2)
                throw new ArgumentException("Boom after delay!!!");

            return "Result is " + ts.ToString();
        }

        internal async static Task<int> GetInt(int ms)
        {
            TimeSpan ts;
            if (ms > 0)
            {
                ts = TimeSpan.FromMilliseconds(ms);
            }
            else
            {
                ts = TimeSpan.FromMilliseconds(-ms);
            }

            await Task.Delay(ts);
            if (ms < 0)
                throw new ArgumentException("Boom after delay!!!");

            return (int)ts.TotalMilliseconds;
        }

        internal async static Task ProcessTask(TimeSpan ts, int throwEx = 0)
        {
            if (throwEx == 1)
                throw new ArgumentException("Boom before delay!!!");

            await Task.Delay(ts);

            if (throwEx == 2)
                throw new ArgumentException("Boom after delay!!!");
        }

    }
}
