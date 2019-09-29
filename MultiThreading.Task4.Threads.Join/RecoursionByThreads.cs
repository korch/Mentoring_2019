using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class RecoursionByThreads : IRecoursionWorker
    {
        public void RunRecoursion()
        {
            var state = 10;
            var thread = new Thread(Recursion);
            thread.Start(state);
            thread.Join();

            Console.WriteLine($"All threads did their job.");
            Console.WriteLine("****************************");
        }

        private void Recursion(object obj)
        {
            var state = (int)obj;

            if (state <= 0) return;

            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} - state:{state}");

            state--;

            var thread = new Thread(Recursion);
            thread.Start(state);
            thread.Join();
        }
    }
}
