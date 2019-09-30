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

        public void Run()
        {
            _list = new List<int>(10);

            var task = Task.Factory.StartNew(AddItem);

            task.Wait();
        }

        private void AddItem()
        {
            for (var i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(PrintCollection);

                lock (_list)
                {
                  
                    _list.Add(i);
                    Console.WriteLine("Added element");
                }  
                auto.Set(); //signal for threads that they can run 
            }

            auto.Close();
            
        }

        private void PrintCollection()
        {
            auto.WaitOne(); // wait a signal from another thread
            Console.WriteLine("New print:");
            lock (_list)
            {
                foreach (var item in _list)
                {
                    Console.WriteLine(item);
                }
            } 
        }
    }
}
