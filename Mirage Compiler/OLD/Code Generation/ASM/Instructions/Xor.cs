using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 xor instruction - Xors value from right operand on the left operand
    /// </summary>
    internal class Xor : Instruction
    {
        public string Left = "";
        public string Right = "";

        public Xor(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public Xor() { }

        public override string ToString()
        {
            return $"xor {Left}, {Right}";
        }
    }
}
