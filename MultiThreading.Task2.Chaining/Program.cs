/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;
using static System.Console;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            WriteLine("First Task – creates an array of 10 random integer.");
            WriteLine("Second Task – multiplies this array with another random integer.");
            WriteLine("Third Task – sorts this array by ascending.");
            WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            WriteLine();

            // feel free to add your code

            var processor = new ArrayProcessor();

            FirstMethod(processor, 10);

            SecondMethod(processor, 15);
            
            ReadLine();
        }

        private static void FirstMethod(ArrayProcessor processor, int arrayCapacity)
        {
            Task<int[]> task = new Task<int[]>(() => processor.CreateArray(arrayCapacity));
            task.Start();

            task.ContinueWith((t) => processor.Multiply(t.Result))
                .ContinueWith((t) => processor.Sort(t.Result))
                .ContinueWith((t) => {
                    var value = processor.GetAverageValue(t.Result);
                    WriteLine($"Average value is: {value}");
                });

            task.Wait();
        }

        private static void SecondMethod(ArrayProcessor processor, int arrayCapacity)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var array = processor.CreateArray(arrayCapacity);
                Task.Factory.StartNew(() =>
                {
                    array = processor.Multiply(array);
                    Task.Factory.StartNew(() =>
                    {
                        array = processor.Sort(array);
                        Task.Factory.StartNew(() => {
                            var value = processor.GetAverageValue(array);
                            WriteLine($"Average value is: {value}");
                        },
                            TaskCreationOptions.AttachedToParent);
                    }, TaskCreationOptions.AttachedToParent);
                },
                    TaskCreationOptions.AttachedToParent);
            });

            task.Wait();
        }
    }
}
