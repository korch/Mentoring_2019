/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code

            a();

            b();

            c();

            d();

            Console.ReadLine();
        }
       
         #region a,b,c,d tasks.
        private static void a()
        {
            // a.
            var task1 = Task.Run(() => Method());
            task1.ContinueWith(Continuation, TaskContinuationOptions.None); // 'None' option sets as default if we don't set any option

            task1.Wait();

            var tcs = new TaskCompletionSource<object>();
            var task2 = tcs.Task;

            tcs.SetException(new CustomException("This exception is expected!"));
            task2.ContinueWith(Continuation, TaskContinuationOptions.None); // 'None' option sets as default if we don't set any option
        }

        private static void b()
        {
            // b. 
            var tcs = new TaskCompletionSource<object>();
            var task = tcs.Task;

            tcs.SetException(new CustomException("This exception is expected!"));
            task.ContinueWith(Continuation, TaskContinuationOptions.OnlyOnFaulted); // without success.

            tcs = new TaskCompletionSource<object>();
            task = tcs.Task;

            tcs.SetCanceled();
            task.ContinueWith(Continuation, TaskContinuationOptions.OnlyOnCanceled); // without success.
        }

        private static void c()
        {
            // c. 
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

        private static void d()
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
        #endregion


        #region output methods
        private static void Method()
        {
            Console.WriteLine($"Task #{Task.CurrentId} did this method in thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine(new string('-', 80));
        }

        private static void Continuation(Task task)
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
        #endregion
    }

    internal class CustomException : Exception
    {
        public CustomException(String message) : base(message)
        { }
    }
}
