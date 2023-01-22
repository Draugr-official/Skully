using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 hlt instruction - Halts execution at point until a flag tells the cpu to continue
    /// </summary>
    internal class Hlt : Instruction
    {
        public override string ToString()
        {
            return $"hlt";
        }
    }
}
