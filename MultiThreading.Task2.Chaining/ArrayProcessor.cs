using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreading.Task2.Chaining
{
    public class ArrayProcessor : IArrayProcessor
    {
        /// <summary>
        /// Creates a new array with specified capacity
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns>created array</returns>
        public int[] CreateArray(int capacity)
        {
            var array = new int[capacity];
            var rand = new Random();

            for (int i = 0; i < capacity; i++)
            {
                var value = rand.Next(1, 10);
                array[i] = value;
            }

            View(array, $"Array was created with capacity:{capacity}");

            return array;
        }

        /// <summary>
        /// Gets average value from an array
        /// </summary>
        /// <param name="array"></param>
        /// <returns>average value of array</returns>
        public decimal GetAverageValue(int[] array)
        {
            var sum = 0;
            for (var i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }

            var average = (decimal)sum / array.Length;

            return average;
        }

        /// <summary>
        /// Multiplies array with random value between 1 and 10
        /// </summary>
        /// <param name="array">array</param>
        /// <returns>multiplied array</returns>
        public int[] Multiply(int[] array)
        {
            var value = new Random().Next(1, 10);

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = array[i] * value;
            }

            View(array, $"Array was multipied with value: {value}");

            return array;
        }

        /// <summary>
        /// Sorts an array by ascending
        /// </summary>
        /// <param name="array">array</param>
        public int[] Sort(int[] array)
        {
            // sorting
            int temp;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                    {
                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }

            View(array, "Array was sorted by Ascending");
            return array;
        }

        /// <summary>
        /// Prints an array on console.
        /// </summary>
        /// <param name="array">array</param>
        private void View(int[] array, string message)
        {
            Console.WriteLine(message);
            Console.Write("Current array: ");

            for (var i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]}|");
            }

            Console.WriteLine();
            Console.WriteLine("*************************************");
            Console.WriteLine();
        }
    }
}
