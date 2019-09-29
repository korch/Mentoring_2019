using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreading.Task6.Continuation
{
    internal class CustomException : Exception
    {
        public CustomException(String message) : base(message)
        { }
    }
}
