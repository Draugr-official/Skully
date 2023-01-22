using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 ret instruction - After call, jumps to location pushed to stack
    /// </summary>
    internal class Ret : Instruction
    {
        public override string ToString()
        {
            return $"ret";
        }
    }
}
