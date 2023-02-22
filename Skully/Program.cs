using System;
using LLVMSharp;
using Skully.Compiler.CodeGen;
using System.Diagnostics;

namespace Skully
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CLI.Parse(args);
        }
    }
}