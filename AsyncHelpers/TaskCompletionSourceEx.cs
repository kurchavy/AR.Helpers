using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Helpers
{
    public static class TaskCompletionSourceEx
    {
        /// <summary>
        /// Try set Task's result to TaskCompletionSource
        /// </summary>
        /// <typeparam name="T">Task' result type</typeparam>
        /// <param name="tcs">TaskCompletionSource object</param>
        /// <param name="task">Task to propagate</param>
        public static void Propagate<T>(this TaskCompletionSource<T> tcs, Task<T> task)
        {
            if (task.Status == TaskStatus.Faulted)
            {
                var aggrEx = task.Exception;
                if (aggrEx.InnerExceptions != null && aggrEx.InnerExceptions.Count > 0)
                    tcs.TrySetException(aggrEx.InnerExceptions[0]);
                else
                    tcs.TrySetException(aggrEx);
            }
            else if (task.Status == TaskStatus.Canceled)
            {
                tcs.TrySetCanceled();
            }
            else if (task.Status == TaskStatus.RanToCompletion)
            {
                tcs.TrySetResult(task.Result);
            }
            else 
            {
                throw new InvalidOperationException(
                    String.Format("Task should be completed. Current task status: {0}", task.Status));
            }
        }

        /// <summary>
        /// Try set Task's result to TaskCompletionSource
        /// </summary>
        /// <param name="tcs">TaskCompletionSource object</param>
        /// <param name="task">Task to propagate</param>
        public static void Propagate(this TaskCompletionSource<object> tcs, Task task)
        {
            if (task.Status == TaskStatus.Faulted)
            {
                var aggrEx = task.Exception;
                if (aggrEx.InnerExceptions != null && aggrEx.InnerExceptions.Count > 0)
                    tcs.TrySetException(aggrEx.InnerExceptions[0]);
                else
                    tcs.TrySetException(aggrEx);
            }
            else if (task.Status == TaskStatus.Canceled)
            {
                tcs.TrySetCanceled();
            }
            else if (task.Status == TaskStatus.RanToCompletion)
            {
                tcs.TrySetResult(null);
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format("Task should be completed. Current task status: {0}", task.Status));
            }
        }

    }
}
