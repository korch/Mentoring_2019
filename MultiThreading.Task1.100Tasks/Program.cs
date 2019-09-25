/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            HundredTasks_FirstMethod();

            HundredTasks_SecondMethod();

            HundredTasks_ThirdMethod();


            Console.ReadLine();
        }


        static void HundredTasks_FirstMethod()
        {
            // feel free to add your code here
            var taskArray = new Task[TaskAmount];

            for (var i = 0; i < taskArray.Length; i++)
            {
                var taskId = i + 1;
                taskArray[i] = new Task(() => { Loop(taskId); });
                taskArray[i].Start();
            }

            Task.WaitAll(taskArray); // we should wait all tasks
        }

        static void HundredTasks_SecondMethod()
        {
            // feel free to add your code here
            var taskArray = new Task[TaskAmount];

            for (var i = 0; i < taskArray.Length; i++)
            {
                var taskId = i + 1;
                taskArray[i] = Task.Factory.StartNew(() => Loop(taskId));
            }

            Task.WaitAll(taskArray); // we should wait all tasks
        }

        static void HundredTasks_ThirdMethod()
        {
            // feel free to add your code here
            var taskArray = new Task[TaskAmount];

            for (var i = 0; i < taskArray.Length; i++)
            {
                var taskId = i + 1;
                taskArray[i] = Task.Run(() => { Loop(taskId); });
            }

            Task.WaitAll(taskArray); // we should wait all tasks
        }

        static void Loop(int taskId)
        {
            for (var j = 1; j <= MaxIterationsCount; j++)
            {
                Output(taskId, j);
            }
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
