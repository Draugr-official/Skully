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
            args = new string[] { "build", "Tests\\hello-world.cs" };
            CLI.Parse(args);
        }
    }
}