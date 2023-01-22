using Mirage_Compiler.Compiler.Code_Generation.ASM;
using Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions;
using Mirage_Compiler.Compiler.Code_Generation.ASM.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.Windows.API
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messageboxa
    /// </summary>
    internal class MessageBoxA : KernelFunction
    {
        public static string Name = "MessageBoxA";
        public static string CsName = "MessageBox.Show";

        public static string HWND = Registers.RCX;
        public static string MessageBoxText = Registers.RDX;
        public static string Caption = Registers.R8;
        public static string MessageBoxButton = Registers.R9D;

        public override void AddCall(ASMContext context, object[] arguments)
        {
            context.AddInstruction(new Mov(Registers.RCX, arguments[0].ToString()));
            context.AddInstruction(new Lea(Registers.RDX, arguments[1].ToString()));
            context.AddInstruction(new Lea(Registers.R8, arguments[2].ToString()));
            context.AddInstruction(new Mov(Registers.R9D, arguments[3].ToString()));
            context.AddInstruction(new Call(Name));
        }

        public static List<string> Arguments = new List<string>() 
        {
            HWND,
            MessageBoxText,
            Caption,
            MessageBoxButton
        };
    }
}
