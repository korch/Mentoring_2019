using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    internal class Worker
    {
        // a
        public void TaskA()
        {

            var task1 = Task.Run(() => Method());
            task1.ContinueWith(Continuation, TaskContinuationOptions.None); // 'None' option sets as default if we don't set any option

            task1.Wait();

            var tcs = new TaskCompletionSource<object>();
            var task2 = tcs.Task;

            tcs.SetException(new CustomException("This exception is expected!"));
            task2.ContinueWith(Continuation, TaskContinuationOptions.None); // 'None' option sets as default if we don't set any option
        }

        public void TaskB()
        {
            var tcs = new TaskCompletionSource<object>();
            var task = tcs.Task;

            tcs.SetException(new CustomException("This exception is expected!"));
            task.ContinueWith(Continuation, TaskContinuationOptions.OnlyOnFaulted); // without success.

            tcs = new TaskCompletionSource<object>();
            task = tcs.Task;

            tcs.SetCanceled();
            task.ContinueWith(Continuation, TaskContinuationOptions.OnlyOnCanceled); // without success.
        }

        public void TaskC()
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine($"Hello from task c main task with tread {Thread.CurrentThread.ManagedThreadId}");
                throw new CustomException(":o");
            });

            task.ContinueWith((t) => {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"Hello from continuation task C! with thread {Thread.CurrentThread.ManagedThreadId}");
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            try
            {
                task.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
        }

        public void TaskD()
        {
            // d.
            var tcs = new TaskCompletionSource<object>();
            var task = tcs.Task;

            tcs.SetCanceled();
            task.ContinueWith((t) =>
            {
                var thread = new Thread(() => Continuation(t));
                Console.WriteLine($"Hello from outside of the thread pool. task Id: {t.Id}");
                thread.Start();

            }, TaskContinuationOptions.OnlyOnCanceled); // without success.
        }

        private void Method()
        {
            Console.WriteLine($"Task #{Task.CurrentId} did this method in thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine(new string('-', 80));
        }

        private void Continuation(Task task)
        {
            if (task.IsFaulted)
            {
                Console.WriteLine($"is faulted | from taskId:{task.Id}");
            }

            if (task.Exception != null)
            {
                Console.WriteLine($"with exception | from taskId:{task.Id}");
            }


            Console.WriteLine($"Continuation from task id: {task.Id}");
            Console.WriteLine($"Continuation from task id: {task.Id} did this method in thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
        }
    }
}
