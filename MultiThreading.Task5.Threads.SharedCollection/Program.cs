/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static Semaphore sem = new Semaphore(1, 1); // hell yeah, this is a good case to use Semaphore, at least I think so! We can create a corridore only for one thread per time xd
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            var list = new List<int>(10);

            var task = Task.Factory.StartNew(() => AddItem(list));

            task.Wait();

            Console.ReadLine();
        }

        private static void AddItem(List<int> list)
        {
            for (var i = 0; i < 10; i++)
            {
                sem.WaitOne();

                Console.WriteLine("Added element");
                list.Add(i);
                sem.Release();

                var task = Task.Factory.StartNew(() => PrintCollection(list));
                // task.Wait();  // if we use wait for this task we will can do add and print methods without any sync objects ;d
            }
        }

        private static void PrintCollection(List<int> list)
        {

            sem.WaitOne();
            Console.WriteLine("New print:");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            sem.Release();
        }
    }
}
