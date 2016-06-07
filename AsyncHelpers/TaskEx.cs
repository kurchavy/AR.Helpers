using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        /// <summary>
        /// Process tasks in completion order. Based on Sergey Teplyakov' post:
        /// http://sergeyteplyakov.blogspot.ru/2015/06/process-tasks-by-completion.html
        /// </summary>
        /// <typeparam name="T">Task result type</typeparam>
        /// <param name="tasks">Tasks sequence</param>
        /// <returns></returns>
        public static IEnumerable<Task<T>> InCompletionOrder<T>(this IEnumerable<Task<T>> tasks)
        {
            var taskList = tasks.ToList();

            var taskCompletions = new TaskCompletionSource<T>[taskList.Count];

            int completedTask = -1;

            for (int i = 0; i < taskList.Count; i++)
            {
                taskCompletions[i] = new TaskCompletionSource<T>();
                taskList[i].ContinueWith(t => 
                {
                    var nextIdx = Interlocked.Increment(ref completedTask);
                    taskCompletions[nextIdx].Propagate(t);
                });
            }

            return taskCompletions.Select(tcs => tcs.Task);
        }

        /// <summary>
        /// Process tasks in completion order. Based on Sergey Teplyakov' post:
        /// http://sergeyteplyakov.blogspot.ru/2015/06/process-tasks-by-completion.html
        /// </summary>
        /// <param name="tasks">Tasks sequence</param>
        /// <returns></returns>
        public static IEnumerable<Task> InCompletionOrder(this IEnumerable<Task> tasks)
        {
            var taskList = tasks.ToList();

            var taskCompletions = new TaskCompletionSource<object>[taskList.Count];

            int completedTask = -1;

            for (int i = 0; i < taskList.Count; i++)
            {
                taskCompletions[i] = new TaskCompletionSource<object>();
                taskList[i].ContinueWith(t =>
                {
                    var nextIdx = Interlocked.Increment(ref completedTask);
                    taskCompletions[nextIdx].Propagate(t);
                });
            }

            return taskCompletions.Select(tcs => tcs.Task);
        }

    }
}
