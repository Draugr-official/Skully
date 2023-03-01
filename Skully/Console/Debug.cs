using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Skully
{
    internal class Debug
    {
        public static bool HasError = false;

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

        public static Exception Error(string message, string suggestion = "", string fileName = "")
        {
            HasError = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("error");

            if (fileName != "")
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(fileName);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("]");
            }
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($": {message}");

            if (suggestion != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"^ {suggestion}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            return new Exception($"{message}[{fileName}]\n^ {suggestion}");
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
                Console.Write("^ : ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(suggestion);
            }
        }

        public static void PrintBuffer(LLVMMemoryBufferRef llvmBuffer)
        {
            IntPtr llvmBufferPtr = LLVM.GetBufferStart(llvmBuffer);
            size_t llvmBufferLength = LLVM.GetBufferSize(llvmBuffer);
            
            string built = "";
            for (IntPtr i = llvmBufferPtr; i.ToInt64() < llvmBufferPtr.ToInt64() + llvmBufferLength; i += 1)
            {
                built += (char)Marshal.ReadByte(i);
            }
            Debug.Log(built, "FROM MEMORY");
        }
    }
}
