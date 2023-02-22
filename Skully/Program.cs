using LLVMSharp;
using Skully.Compiler.CodeGen;
using System.Diagnostics;

namespace Skully
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for(; ; )
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string File = @"using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(""Hello, World!"");
        Console.WriteLine(""How are you?"", 1231);
    }
}";

                CodeGenConfig config = new CodeGenConfig()
                {
                    Name = "TestProject"
                };
                CodeGenerator codeGenerator = new CodeGenerator(File, config);
                codeGenerator.GenerateLLVM();

                Debug.Log("Compile files? Hit enter");
                Console.ReadLine();

                ProcessStartInfo clang = new ProcessStartInfo();
                clang.FileName = @"C:\Windows\system32\cmd.exe";
                clang.Arguments = $"/c clang out.ll -o {config.Name}.exe";
                Process clangProcess = Process.Start(clang);
                clangProcess.WaitForExit();

                Debug.Success($"Built app to {config.Name}.exe");
                sw.Stop();
                Debug.Log($"Elapsed {sw.Elapsed.TotalMilliseconds}ms");

                Debug.Log("Run executeable? Hit enter");
                Console.ReadLine();

                Process ah = Process.Start($"{config.Name}.exe");
                ah.WaitForExit();

                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}