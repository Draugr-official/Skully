using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 ud instruction - undefined instruction
    /// </summary>
    internal class Ud : Instruction
    {
        public override string ToString()
        {
            return "ud";
        }
    }
}
