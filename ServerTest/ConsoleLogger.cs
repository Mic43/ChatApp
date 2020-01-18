using System;

namespace ServerTest
{
    class ConsoleLogger : ILogger
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}