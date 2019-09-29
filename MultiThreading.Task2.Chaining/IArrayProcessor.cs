using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreading.Task2.Chaining
{
    public interface IArrayProcessor
    {
        int[] Sort(int[] array);
        int[] CreateArray(int capacity);
        int[] Multiply(int[] array);
        decimal GetAverageValue(int[] array);
    }
}
