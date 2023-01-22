using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 call instruction - Stores current location on stack, jumps to label and jumps back to original location upon hitting 'ret'
    /// </summary>
    internal class Call : Instruction
    {
        public string Label = "";

        public Call(string label)
        {
            Label = label;
        }

        public Call() { }

        public override string ToString()
        {
            return $"call {Label}";
        }
    }
}
