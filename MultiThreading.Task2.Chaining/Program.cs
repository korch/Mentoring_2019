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
        static int[] _array;
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            FirstMethod();
            WriteLine("*************************************");

            SecondMethod();
            WriteLine("*************************************");

            ThirdMethod();
            WriteLine("*************************************");

            Console.ReadLine();
        }

        private static void FirstMethod()
        {
            var task = Task.Factory.StartNew(CreateArray)
                .ContinueWith(MultiplyArray)
                .ContinueWith(Sort)
                .ContinueWith(Sum);

            task.Wait();
        }

        private static void SecondMethod()
        {
            var task = Task.Factory.StartNew(() =>
            {
                CreateArray();
                Task.Factory.StartNew(() =>
                {
                    MultiplyArray(Task.CompletedTask);
                    Task.Factory.StartNew(() =>
                    {
                        Sort(Task.CompletedTask);
                        Task.Factory.StartNew(() => { Sum(Task.CompletedTask); },
                            TaskCreationOptions.AttachedToParent);
                    }, TaskCreationOptions.AttachedToParent);
                },
                    TaskCreationOptions.AttachedToParent);
            });

            task.Wait();
        }

        private static void ThirdMethod()
        {
            var tasks = new Task[4]
            {
                new Task(() => CreateArray()),
                new Task(() => MultiplyArray(Task.CompletedTask)),
                new Task(() => Sort(Task.CompletedTask)),
                new Task(() => Sum(Task.CompletedTask))
            };

            foreach (var task in tasks)
            {
                task.Start();
                task.Wait();
            }

            Task.WaitAll(tasks);

        }

        private static int[] CreateArray()
        {
            _array = new int[10];
            var rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                var value = rand.Next(1, 10);
                _array[i] = value;
            }

            WriteLine("Array is created:");
            ViewArray();

            return _array;
        }

        private static Task MultiplyArray(Task task)
        {
            var value = new Random().Next(1, 10);

            for (var i = 0; i < _array.Length; i++)
            {
                _array[i] = _array[i] * value;
            }

            WriteLine();
            WriteLine();
            WriteLine($"Array was multiplied with value:{value}");
            ViewArray();

            return task;
        }

        private static Task Sort(Task task)
        {
            // sorting
            int temp;
            for (int i = 0; i < _array.Length - 1; i++)
            {
                for (int j = i + 1; j < _array.Length; j++)
                {
                    if (_array[i] > _array[j])
                    {
                        temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = temp;
                    }
                }
            }

            WriteLine();
            WriteLine();
            WriteLine("Array was sorted:");
            ViewArray();

            return task;
        }


        private static Task Sum(Task task)
        {
            var sum = 0;
            for (var i = 0; i < _array.Length; i++)
            {
                sum += _array[i];
            }

            var average = (decimal)sum / _array.Length;

            WriteLine();
            WriteLine();
            WriteLine($"Average value: {average}");

            return task;
        }

        private static void ViewArray()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                Write($"{_array[i]}|");
            }
        }
    }
}
