using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.PEBuilder
{
    internal class PE
    {
        public string GccPath = @"C:\MinGW\bin\gcc.exe";
        public string GccDirectoryPath = @"C:\MinGW\bin";

        public string ClangPath = @"C:\Program Files\LLVM\bin\clang.exe";
        public string ClangDirectoryPath = "C:\\Program Files\\LLVM\\bin";

        public void Build(string src)
        {
            DebugOut.Info("Writing out.ll to " + this.ClangDirectoryPath);
            File.WriteAllText(this.ClangDirectoryPath + @"\out.ll", src);

            DebugOut.Info("Launching clang...");
            Process clangProcess = Process.Start(this.ClangPath, "\"C:\\Program Files\\LLVM\\bin\\out.ll\" -o out.exe");
            clangProcess.WaitForExit();
            DebugOut.Info("out.ll built to out.exe!");

            DebugOut.Info("Finished building PE!");
        }
    }
}
