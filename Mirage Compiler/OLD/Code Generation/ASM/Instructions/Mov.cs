using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 mov instruction - Moves value from right operand into left operand
    /// </summary>
    internal class Mov : Instruction
    {
        public string Left = "";
        public string Right = "";

        public Mov(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public Mov() { }

        public override string ToString()
        {
            return $"mov {Left}, {Right}";
        }
    }
}
