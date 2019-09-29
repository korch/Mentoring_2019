using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class RecoursionByThreadPool : IRecoursionWorker
    {
        private Semaphore sem = new Semaphore(1, 2);
        public void RunRecoursion()
        {
            var worker = new ThreadPoolWorker(Recoursion);
            var state = 10;

            // To be honest, I don't know how Semaphore can help me to wait until all threads will finish their work, 
            //so I created a new class where I can check a state of thread and wait until all threads will do their job..
            // Also I included Semaphore and set to him to allow acces for four thread in time... I hope I do it as it should works :D
            worker.Start(state);
            worker.Wait();

            Console.WriteLine($"All threads from Thread pool did their job.");
            Console.WriteLine("****************************");
        }

        private void Recoursion(object obj)
        {
            sem.WaitOne();

            var state = (int)obj;

            if (state <= 0) return;

            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId} - state:{state}");

            state--;

            ThreadPool.QueueUserWorkItem(new WaitCallback(t => { Recoursion(state); }));

            sem.Release();
        }
    }
}
