/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Semaphore sem = new Semaphore(2, 4);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code
            var state = 10;
            var thread = new Thread(BodyForThread);
            thread.Start(state);
            thread.Join();

            Console.WriteLine($"All threads did their job.");
            Console.WriteLine("****************************");

            var worker = new ThreadPoolWorker(BodyForThreadPool);

            // To be honest, I don't know how Semaphore can help me to wait until all threads will finish their work, 
            //so I created a new class where I can check a state of thread and wait until all threads will do their job..
            // Also I included Semaphore and set to him to allow acces for four thread in time... I hope I do it as it should works :D
            worker.Start(state);
            worker.Wait();
           
            Console.WriteLine($"All threads from Thread pool did their job.");
            Console.ReadLine();
        }

        private static void BodyForThread(object obj)
        {
            var state = (int)obj;

            if (state <= 0) return;

            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} - state:{state}");

            state--;

            var thread = new Thread(BodyForThread);
            thread.Start(state);
            thread.Join();
        }

        private static void BodyForThreadPool(object obj)
        {
            sem.WaitOne();

            var state = (int)obj;

            if (state <= 0) return;

            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} - state:{state}");

            state--;

            ThreadPool.QueueUserWorkItem(new WaitCallback(t => { BodyForThreadPool(state); }));

            sem.Release();
        }
    }
}

