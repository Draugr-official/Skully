using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 add instruction - Adds value from right operand to the left operand
    /// </summary>
    internal class Add : Instruction
    {
        public string Left = "";
        public string Right = "";

        public Add(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public Add() { }

        public override string ToString()
        {
            return $"add {Left}, {Right}";
        }
    }
}
