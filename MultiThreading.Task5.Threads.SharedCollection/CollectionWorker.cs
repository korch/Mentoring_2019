using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    internal class CollectionWorker
    {

        private AutoResetEvent auto = new AutoResetEvent(false);
        private List<int> _list;
        private const int Capacity = 10;
        private bool _isRunning = true; // flag for thread of printing manipulation

        private object _lockObj = new object();

        public void Run()
        {
            _list = new List<int>(10);

            //var threadForPrinting = new Thread(new ThreadStart(PrintCollection));
            //threadForPrinting.Start();

            //var task = Task.Factory.StartNew(AddItem);

            //task.Wait();

            AddItemV2();

            Console.WriteLine("Press any key to exit...");

            //auto.Dispose();
            Console.ReadLine();
        }

        private void AddItem()
        {
            for (var i = 0; i < Capacity; i++)
            {
                lock (_list)
                {
                    _list.Add(i);
                    Console.WriteLine("Added element");
                }

                if (i == Capacity - 1)
                {
                    _isRunning = false; // set flag to false after last iteration
                }

                auto.Set(); //signal for threads that they can run 

                Thread.Sleep(1000);
            }
        }

        private void AddItemV2()
        {
            var tasks = new List<Task>();

            for (int i = 0; i <= Capacity; i++)
            {
                var tempI = i;
                var task = Task.Factory.StartNew(() => {
                    lock (_lockObj)
                    {
                        _list.Add(tempI);
                        PrintCollectionV2(tempI);
                    }
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        private void PrintCollectionV2(int elementsCount)
        {
            Console.WriteLine($"Array (when {elementsCount} have been added): {String.Join(", ", _list.ToArray())}");
        }

        private void PrintCollection()
        {
            while (_isRunning)
            {
                auto.WaitOne(); // wait a signal from another thread

                lock (_list)
                {
                    Console.WriteLine("New print:");
                    foreach (var item in _list)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
        }
    }
}

