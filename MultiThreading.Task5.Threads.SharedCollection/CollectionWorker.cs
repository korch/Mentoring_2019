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
        private Queue<int> _listToRan = new Queue<int>();
        private bool _isRunning = true;
        private Thread _threadForPrinting;

        public void Run()
        {
            _list = new List<int>(10);

            var task = Task.Factory.StartNew(AddItem);

            task.Wait();

            Console.WriteLine("Press any key to exit...");
           
            Console.ReadKey(true);
           
        }

        private void AddItem()
        {
            _threadForPrinting = new Thread(new ThreadStart(PrintCollection));
            _threadForPrinting.Start();

            for (var i = 0; i < 10; i++)
            {

                lock (_list)
                {

                    _list.Add(i);
                    _listToRan.Enqueue(i);
                    Console.WriteLine("Added element");
                }

                auto.Set(); //signal for threads that they can run 

                Thread.Sleep(1000);
            }

            _isRunning = false;
            auto.Close();
        }

        private void PrintCollection()
        {
            auto.WaitOne();  // wait a signal from another thread
            while (_isRunning)
            {
                if (_listToRan.Count > 0)
                {    
                    lock (_list)
                    {
                        Console.WriteLine("New print:");
                        foreach (var item in _list)
                        {
                            Console.WriteLine(item);
                        }

                        _listToRan.Dequeue();
                    }
                }
            }
        }
    }
}

