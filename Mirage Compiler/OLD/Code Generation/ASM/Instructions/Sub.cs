using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 sub instruction - Subtracts value from right operand by left operand
    /// </summary>
    internal class Sub : Instruction
    {
        public string Left = "";
        public string Right = "";

        public Sub(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public Sub() { }

        public override string ToString()
        {
            return $"sub {Left}, {Right}";
        }
    }
}
