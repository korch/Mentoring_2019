using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class WorkLoadImitator : IWorkLoad
    {
        public void DoWork()
        {
            Thread.Sleep(new Random().Next(100, 3000));
        }
    }
}
