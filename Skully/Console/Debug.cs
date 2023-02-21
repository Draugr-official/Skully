using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully
{
    internal class Debug
    {
        public static void Log(string message, string suggestion = "")
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("info: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);

            if (suggestion != "")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("suggestion: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(suggestion);
            }
        }

        public static void Error(string message, string suggestion = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("error: ");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;

            if (suggestion != "")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("suggestion -> ");
                Console.WriteLine(suggestion);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void Warn(string message, string suggestion = "")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("warning: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);

            if (suggestion != "")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("suggestion: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(suggestion);
            }
        }

        public static void Success(string message, string suggestion = "")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("success: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);

            if (suggestion != "")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("suggestion: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(suggestion);
            }
        }
    }
}
