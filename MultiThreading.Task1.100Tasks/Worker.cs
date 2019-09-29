using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    internal class Worker
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        public void HundredTasks_FirstMethod()
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

        public void HundredTasks_SecondMethod()
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

        public void HundredTasks_ThirdMethod()
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

        void Loop(int taskId)
        {
            for (var j = 1; j <= MaxIterationsCount; j++)
            {
                Output(taskId, j);
            }
        }

        void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
