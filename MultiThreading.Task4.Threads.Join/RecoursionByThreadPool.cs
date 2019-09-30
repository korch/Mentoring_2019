using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class RecoursionByThreadPool : IRecoursionWorker
    {
        private SemaphoreSlim sem = new SemaphoreSlim(10,10);
        public void RunRecoursion()
        {
            var state = 10;

            ThreadPool.QueueUserWorkItem(new WaitCallback(Recoursion), state);
        
            while (sem.CurrentCount > 0)
            { }

            Console.WriteLine($"All threads from Thread pool did their job.");
            Console.WriteLine("****************************");

            sem.Dispose();
        }

        private void Recoursion(object obj)
        {
            var state = (int)obj;

            if (state <= 0) return;

            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} - state:{state}");

            state--;
            ThreadPool.QueueUserWorkItem(new WaitCallback(t => { Recoursion(state); }));

            sem.Wait();
        }
    }
}
