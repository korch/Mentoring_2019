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

            // a.
            var task1 = Task.Run(() => Method());
            task1.ContinueWith((t) => Continuation(t), TaskContinuationOptions.None); // 'None' option sets as default if we don't set any option

            task1.Wait();

            var tcs = new TaskCompletionSource<object>();
            var task2 = tcs.Task;

            tcs.SetException(new CustomException("This exception is expected!"));
            task2.ContinueWith((t) => Continuation(t), TaskContinuationOptions.None); // 'None' option sets as default if we don't set any option

         

            // b. 
            tcs = new TaskCompletionSource<object>();
            var task3 = tcs.Task;

            tcs.SetException(new CustomException("This exception is expected!"));
            task3.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnFaulted); // without success.

            tcs = new TaskCompletionSource<object>();
            var task4 = tcs.Task;

            tcs.SetCanceled();
            task4.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnCanceled); // without success.


            // c.


            Console.ReadLine();
        }


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
    }

    internal class CustomException : Exception
    {
        public CustomException(String message) : base(message)
        { }
    }
}
