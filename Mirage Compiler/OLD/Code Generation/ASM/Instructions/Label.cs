using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions
{
    /// <summary>
    /// x64 label
    /// </summary>
    internal class Label : Instruction
    {
        public string Name = "";

        public Label(string name)
        {
            Name = name;
        }

        public Label() { }

        public override string ToString()
        {
            return $"{this.Name}:";
        }
    }
}
