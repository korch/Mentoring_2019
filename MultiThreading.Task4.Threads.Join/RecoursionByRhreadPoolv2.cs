using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class RecoursionByRhreadPoolv2 : IRecoursionWorker
    {
        private SemaphoreSlim sem = new SemaphoreSlim(0, 1);
        public void RunRecoursion()
        {
            ThreadPool.QueueUserWorkItem((t) => {

                Recoursion(t);
                sem.Release();

            }, 10);
            sem.Wait();

            Console.WriteLine($"All threads from Thread pool did their job.");
            Console.WriteLine("****************************");
        }

        private void Recoursion(object obj)
        {
            var state = (int)obj;

            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} - state:{state}");

            state--;

            if (state <= 0) return;

            var semaphoreSlim = new SemaphoreSlim(0, 1);
            ThreadPool.QueueUserWorkItem((t) =>
            {
                Recoursion(t);
                semaphoreSlim.Release();
            }, state);

            semaphoreSlim.Wait();
        }
    }
}
