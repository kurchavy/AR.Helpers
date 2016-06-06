using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Helpers
{
    public static class TaskEx
    {
        /// <summary>
        /// Execute task with timeout
        /// </summary>
        /// <typeparam name="T">Task result type</typeparam>
        /// <param name="task">Task</param>
        /// <param name="timeout">Timeout value</param>
        /// <returns></returns>
        public static Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<T>();

            task.ContinueWith(t => tcs.Propagate(task), 
                TaskContinuationOptions.ExecuteSynchronously);

            Task.Delay(timeout).ContinueWith(t => tcs.TrySetCanceled(), 
                TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        /// <summary>
        /// Execute task with timeout
        /// </summary>
        /// <param name="task">Task to execute</param>
        /// <param name="timeout">Timeout value</param>
        /// <returns></returns>
        public static Task WithTimeout(this Task task, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<object>();

            task.ContinueWith(t => tcs.Propagate(task), 
                TaskContinuationOptions.ExecuteSynchronously);

            Task.Delay(timeout)
                .ContinueWith(t => tcs.TrySetCanceled(),
                    TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }
    }
}
