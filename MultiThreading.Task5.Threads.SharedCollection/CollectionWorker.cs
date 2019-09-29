using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    internal class CollectionWorker
    {
        private Semaphore _sem = new Semaphore(1, 1); // hell yeah, this is a good case to use Semaphore, 
                                                     //at least I think so! We can create a corridore only for one thread per time xd

        private List<int> _list;

        public void Run()
        {
            _list = new List<int>(10);

            var task = Task.Factory.StartNew(() => AddItem());

            task.Wait();
        }

        private void AddItem()
        {
            for (var i = 0; i < 10; i++)
            {
                _sem.WaitOne();

                Console.WriteLine("Added element");
                _list.Add(i);
                _sem.Release();

                var task = Task.Factory.StartNew(() => PrintCollection());
                // task.Wait();  // if we use wait for this task we will can do add and print methods without any sync objects ;d
            }
        }

        private void PrintCollection()
        {

            _sem.WaitOne();
            Console.WriteLine("New print:");
            foreach (var item in _list)
            {
                Console.WriteLine(item);
            }
            _sem.Release();
        }
    }
}
