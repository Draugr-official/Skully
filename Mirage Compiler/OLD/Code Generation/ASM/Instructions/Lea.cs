using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 lea instruction - Load effective address
    /// </summary>
    internal class Lea : Instruction
    {
        public string Left = "";
        public string Right = "";

        public Lea(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public Lea() { }

        public override string ToString()
        {
            return $"lea {Left}, {Right}";
        }
    }
}
