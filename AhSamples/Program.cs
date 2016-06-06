using AR.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var prg = new Program();
            prg.TestTimeout();
        }

        private void TestTimeout()
        {
            var t1 = GetResult(1, TimeSpan.FromMilliseconds(1000));
            var t2 = GetResult(2, TimeSpan.FromMilliseconds(2100));

            var r1 = Task.Run(async () =>
            {
                try
                {
                    await t1.WithTimeout(TimeSpan.FromSeconds(2));
                    Console.WriteLine(t1.Result);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task T1 cancelled");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            var r2 = Task.Run(async () =>
            {
                try
                {
                    await t2.WithTimeout(TimeSpan.FromSeconds(2));
                    Console.WriteLine(t2.Result);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task T2 cancelled");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            
            Console.ReadLine();
        }



        private async Task<string> GetResult(int arg, TimeSpan ts)
        {
            await Task.Delay(ts);
            return "Result is " + arg.ToString();
        }
    }
}
