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
            //prg.TestTimeout();
            //prg.TestOrderNaive();
            prg.TestOrder();
            Console.ReadLine();
        }

        private void TestTimeout()
        {
            var t1 = GetResult(TimeSpan.FromMilliseconds(2000));
            var t2 = GetResult(TimeSpan.FromMilliseconds(5000));
            var t3 = GetResult(TimeSpan.FromMilliseconds(5000), 2);

            t2.ContinueWith(t => Console.WriteLine(t.Result));

            var r1 = Task.Run(async () =>
            {
                try
                {
                    await t1.WithTimeout(TimeSpan.FromSeconds(3));
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
                    await t2.WithTimeout(TimeSpan.FromSeconds(3));
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

            var r3 = Task.Run(async () =>
            {
                try
                {
                    await t3.WithTimeout(TimeSpan.FromSeconds(3));
                    Console.WriteLine(t3.Result);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task T3 cancelled");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            
        }

        private void TestOrder()
        {
            var t1 = GetResult(TimeSpan.FromMilliseconds(7000));
            var t2 = GetResult(TimeSpan.FromMilliseconds(2000));
            var t3 = GetResult(TimeSpan.FromMilliseconds(3000), 1);
            
            var tasks = new Task<string>[] { t1, t2, t3 };

            //TestOrderNaive(tasks);
            TestOrderCompletion(tasks);
        }

        private async void TestOrderNaive(Task<string>[] tasks)
        {
            foreach (var t in tasks)
            {
                try
                {
                    var res = await t;
                    Console.WriteLine(res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }

        private async void TestOrderCompletion(Task<string>[] tasks)
        {
            foreach (var t in tasks.InCompletionOrder())
            {
                try
                {
                    var res = await t;
                    Console.WriteLine(res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task<string> GetResult(TimeSpan ts, int throwEx = 0)
        {
            if (throwEx == 1)
                throw new ArgumentException("Boom before delay!!!");

            await Task.Delay(ts);

            if (throwEx == 2)
                throw new ArgumentException("Boom after delay!!!");

            return "Result is " + ts.ToString();
        }
    }
}
